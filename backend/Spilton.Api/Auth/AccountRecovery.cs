using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Spilton.Api.Data;

namespace Spilton.Api.Auth;

public sealed class AccountChallenge
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public string Purpose { get; set; } = "PASSWORD_RESET";
    public string CodeSalt { get; set; } = "";
    public string CodeHash { get; set; } = "";
    public string? ResetTokenHash { get; set; }
    public int FailedAttempts { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset ExpiresAt { get; set; }
    public DateTimeOffset? VerifiedAt { get; set; }
    public DateTimeOffset? UsedAt { get; set; }
}

public static class AccountRecoveryModel
{
    public static void Configure(ModelBuilder model)
    {
        var challenges = model.Entity<AccountChallenge>();
        challenges.HasKey(x => x.Id);
        challenges.HasOne<User>().WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
        challenges.Property(x => x.Purpose).HasMaxLength(32);
        challenges.Property(x => x.CodeSalt).HasMaxLength(32);
        challenges.Property(x => x.CodeHash).HasMaxLength(64);
        challenges.Property(x => x.ResetTokenHash).HasMaxLength(64);
        challenges.HasIndex(x => new { x.UserId, x.Purpose, x.CreatedAt });
        challenges.HasIndex(x => x.ResetTokenHash).IsUnique();
    }
}

public sealed class EmailSettings
{
    public string Provider { get; set; } = "Disabled";
    public string SmtpHost { get; set; } = "";
    public int SmtpPort { get; set; } = 587;
    public string SmtpUsername { get; set; } = "";
    public string SmtpPassword { get; set; } = "";
    public string FromAddress { get; set; } = "";
    public string FromName { get; set; } = "Spilton AI";
    public bool EnableSsl { get; set; } = true;
}

public interface IAccountEmailSender
{
    bool Available { get; }
    Task SendPasswordResetCode(string email, string name, string code, CancellationToken ct);
    Task SendEmailVerificationCode(string email, string name, string code, CancellationToken ct);
}

public sealed class SmtpAccountEmailSender(EmailSettings settings) : IAccountEmailSender
{
    public bool Available => settings.Provider.Equals("Smtp", StringComparison.OrdinalIgnoreCase)
        && !string.IsNullOrWhiteSpace(settings.SmtpHost) && settings.SmtpPort is > 0 and <= 65535
        && MailAddress.TryCreate(settings.FromAddress, out _);

    public async Task SendPasswordResetCode(string email, string name, string code, CancellationToken ct)
    {
        if (!Available) throw new InvalidOperationException("Email provider is not configured.");
        using var message = new MailMessage
        {
            From = new MailAddress(settings.FromAddress, settings.FromName),
            Subject = "Your Spilton password reset code",
            Body = $"Hello {name},\n\nYour Spilton password reset code is: {code}\n\nThis code expires in 10 minutes and can be used once. If you did not request it, ignore this email.",
            IsBodyHtml = false
        };
        message.To.Add(email);
        using var client = new SmtpClient(settings.SmtpHost, settings.SmtpPort)
        {
            EnableSsl = settings.EnableSsl,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = string.IsNullOrWhiteSpace(settings.SmtpUsername),
            Credentials = string.IsNullOrWhiteSpace(settings.SmtpUsername) ? CredentialCache.DefaultNetworkCredentials : new NetworkCredential(settings.SmtpUsername, settings.SmtpPassword)
        };
        await client.SendMailAsync(message, ct);
    }
    public async Task SendEmailVerificationCode(string email, string name, string code, CancellationToken ct)
    {
        if (!Available) throw new InvalidOperationException("Email provider is not configured.");
        using var message = new MailMessage { From = new MailAddress(settings.FromAddress, settings.FromName), Subject = "Verify your Spilton email", Body = $"Hello {name},\n\nYour Spilton email verification code is: {code}\n\nThis code expires in 10 minutes and can be used once.", IsBodyHtml = false };
        message.To.Add(email);
        using var client = new SmtpClient(settings.SmtpHost, settings.SmtpPort) { EnableSsl = settings.EnableSsl, DeliveryMethod = SmtpDeliveryMethod.Network, UseDefaultCredentials = string.IsNullOrWhiteSpace(settings.SmtpUsername), Credentials = string.IsNullOrWhiteSpace(settings.SmtpUsername) ? CredentialCache.DefaultNetworkCredentials : new NetworkCredential(settings.SmtpUsername, settings.SmtpPassword) };
        await client.SendMailAsync(message, ct);
    }
}

public sealed record ForgotPasswordRequest([Required, EmailAddress, StringLength(254)] string Email);
public sealed record VerifyResetCodeRequest([Required, EmailAddress, StringLength(254)] string Email, [Required, RegularExpression("^[0-9]{6}$")] string Code);
public sealed record ResetPasswordRequest([Required, StringLength(128, MinimumLength = 64)] string ResetToken, [Required, StringLength(128, MinimumLength = 6)] string NewPassword);
public sealed record UpdateProfileRequest([Required, StringLength(100, MinimumLength = 1)] string Name);
public sealed record VerifyEmailRequest([Required, RegularExpression("^[0-9]{6}$")] string Code);

[ApiController, Route("api/auth"), ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
public sealed class AccountRecoveryController(AppDbContext db, IPasswordHasher<User> hasher, IAccountEmailSender email, TimeProvider clock, ILogger<AccountRecoveryController> logger) : ControllerBase
{
    private static string Hash(string value) => Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(value)));
    private static string CodeHash(string salt, string code) => Hash(salt + ":" + code);
    private Guid UserId => Guid.Parse(User.FindFirst("sub")!.Value);

    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "User"), HttpPost("request-email-verification"), EnableRateLimiting("auth")]
    public async Task<IActionResult> RequestEmailVerification(CancellationToken ct)
    {
        if (!email.Available) return Problem(statusCode: 503, title: "Verification email is not configured yet.");
        var user = await db.Users.SingleAsync(x => x.Id == UserId && x.IsActive, ct);
        if (user.EmailVerifiedAt is not null) return NoContent();
        var now = clock.GetUtcNow();
        await db.Set<AccountChallenge>().Where(x => x.UserId == user.Id && x.Purpose == "EMAIL_VERIFICATION" && x.UsedAt == null).ExecuteUpdateAsync(s => s.SetProperty(x => x.UsedAt, now), ct);
        var code = RandomNumberGenerator.GetInt32(0, 1_000_000).ToString("D6"); var salt = Convert.ToHexString(RandomNumberGenerator.GetBytes(16));
        db.Add(new AccountChallenge { UserId = user.Id, Purpose = "EMAIL_VERIFICATION", CodeSalt = salt, CodeHash = CodeHash(salt, code), CreatedAt = now, ExpiresAt = now.AddMinutes(10) }); await db.SaveChangesAsync(ct);
        try { await email.SendEmailVerificationCode(user.Email, user.Name, code, ct); }
        catch (Exception ex) { await db.Set<AccountChallenge>().Where(x => x.UserId == user.Id && x.Purpose == "EMAIL_VERIFICATION" && x.UsedAt == null).ExecuteUpdateAsync(s => s.SetProperty(x => x.UsedAt, now), CancellationToken.None); logger.LogWarning("Email verification delivery failed. ErrorType={ErrorType}", ex.GetType().Name); return Problem(statusCode: 503, title: "The verification email could not be sent. Please try again shortly."); }
        return Accepted(new { message = "A six-digit verification code has been sent." });
    }

    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "User"), HttpPost("verify-email"), EnableRateLimiting("auth")]
    public async Task<ActionResult<UserResponse>> VerifyEmail(VerifyEmailRequest request, CancellationToken ct)
    {
        var now=clock.GetUtcNow();var user=await db.Users.Include(x=>x.Roles).SingleAsync(x=>x.Id==UserId&&x.IsActive,ct);
        if(user.EmailVerifiedAt is not null)return Ok(UserResponse.From(user));
        var challenge=await db.Set<AccountChallenge>().Where(x=>x.UserId==user.Id&&x.Purpose=="EMAIL_VERIFICATION"&&x.UsedAt==null&&x.ExpiresAt>now).OrderByDescending(x=>x.CreatedAt).FirstOrDefaultAsync(ct);
        if(challenge is null||challenge.FailedAttempts>=5||!CryptographicOperations.FixedTimeEquals(Convert.FromHexString(challenge.CodeHash),Convert.FromHexString(CodeHash(challenge.CodeSalt,request.Code)))){if(challenge is not null){challenge.FailedAttempts++;if(challenge.FailedAttempts>=5)challenge.UsedAt=now;await db.SaveChangesAsync(ct);}return Problem(statusCode:400,title:"The code is invalid or has expired.");}
        var consumed=await db.Set<AccountChallenge>().Where(x=>x.Id==challenge.Id&&x.UsedAt==null&&x.ExpiresAt>now).ExecuteUpdateAsync(s=>s.SetProperty(x=>x.UsedAt,now).SetProperty(x=>x.VerifiedAt,now),ct);if(consumed!=1)return Problem(statusCode:400,title:"The code is invalid or has expired.");
        user.EmailVerifiedAt=now;await db.SaveChangesAsync(ct);return Ok(UserResponse.From(user));
    }

    [HttpPost("forgot-password"), EnableRateLimiting("auth")]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest request, CancellationToken ct)
    {
        if (!email.Available) return Problem(statusCode: 503, title: "Password recovery email is not configured yet.");
        var normalized = request.Email.Trim().ToUpperInvariant();
        var user = await db.Users.SingleOrDefaultAsync(x => x.NormalizedEmail == normalized && x.IsActive, ct);
        if (user is not null)
        {
            var now = clock.GetUtcNow();
            await db.Set<AccountChallenge>().Where(x => x.UserId == user.Id && x.Purpose == "PASSWORD_RESET" && x.UsedAt == null)
                .ExecuteUpdateAsync(s => s.SetProperty(x => x.UsedAt, now), ct);
            var code = RandomNumberGenerator.GetInt32(0, 1_000_000).ToString("D6");
            var salt = Convert.ToHexString(RandomNumberGenerator.GetBytes(16));
            db.Add(new AccountChallenge { UserId = user.Id, CodeSalt = salt, CodeHash = CodeHash(salt, code), CreatedAt = now, ExpiresAt = now.AddMinutes(10) });
            await db.SaveChangesAsync(ct);
            try { await email.SendPasswordResetCode(user.Email, user.Name, code, ct); }
            catch (Exception ex)
            {
                await db.Set<AccountChallenge>().Where(x => x.UserId == user.Id && x.UsedAt == null).ExecuteUpdateAsync(s => s.SetProperty(x => x.UsedAt, now), CancellationToken.None);
                logger.LogWarning("Password recovery delivery failed. ErrorType={ErrorType}", ex.GetType().Name);
                return Problem(statusCode: 503, title: "The recovery email could not be sent. Please try again shortly.");
            }
        }
        return Accepted(new { message = "If an active account uses that email, a six-digit reset code has been sent." });
    }

    [HttpPost("verify-reset"), EnableRateLimiting("auth")]
    public async Task<IActionResult> VerifyReset(VerifyResetCodeRequest request, CancellationToken ct)
    {
        var normalized = request.Email.Trim().ToUpperInvariant();
        var now = clock.GetUtcNow();
        var challenge = await db.Set<AccountChallenge>()
            .Where(x => x.Purpose == "PASSWORD_RESET" && x.VerifiedAt == null && x.UsedAt == null && x.ExpiresAt > now)
            .Join(db.Users.Where(u => u.NormalizedEmail == normalized && u.IsActive), x => x.UserId, u => u.Id, (x, _) => x)
            .OrderByDescending(x => x.CreatedAt).FirstOrDefaultAsync(ct);
        if (challenge is null || challenge.FailedAttempts >= 5 || !CryptographicOperations.FixedTimeEquals(Convert.FromHexString(challenge.CodeHash), Convert.FromHexString(CodeHash(challenge.CodeSalt, request.Code))))
        {
            if (challenge is not null) { challenge.FailedAttempts++; if (challenge.FailedAttempts >= 5) challenge.UsedAt = now; await db.SaveChangesAsync(ct); }
            return Problem(statusCode: 400, title: "The code is invalid or has expired.");
        }
        var token = Convert.ToHexString(RandomNumberGenerator.GetBytes(48));
        var verified = await db.Set<AccountChallenge>().Where(x => x.Id == challenge.Id && x.VerifiedAt == null && x.UsedAt == null && x.ExpiresAt > now)
            .ExecuteUpdateAsync(s => s.SetProperty(x => x.ResetTokenHash, Hash(token)).SetProperty(x => x.VerifiedAt, now), ct);
        if (verified != 1) return Problem(statusCode: 400, title: "The code is invalid or has expired.");
        return Ok(new { resetToken = token });
    }

    [HttpPost("reset-password"), EnableRateLimiting("auth")]
    public async Task<IActionResult> ResetPassword(ResetPasswordRequest request, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.NewPassword)) return BadRequest(new { title = "A non-blank password is required." });
        var now = clock.GetUtcNow(); var hash = Hash(request.ResetToken);
        await using var transaction = await db.Database.BeginTransactionAsync(ct);
        var challenge = await db.Set<AccountChallenge>().SingleOrDefaultAsync(x => x.ResetTokenHash == hash && x.VerifiedAt != null && x.UsedAt == null && x.ExpiresAt > now, ct);
        if (challenge is null) return Problem(statusCode: 400, title: "The reset session is invalid or has expired.");
        var consumed = await db.Set<AccountChallenge>().Where(x => x.Id == challenge.Id && x.UsedAt == null && x.ExpiresAt > now)
            .ExecuteUpdateAsync(s => s.SetProperty(x => x.UsedAt, now), ct);
        if (consumed != 1) return Problem(statusCode: 400, title: "The reset session is invalid or has expired.");
        var user = await db.Users.Include(x => x.Roles).SingleAsync(x => x.Id == challenge.UserId && x.IsActive, ct);
        user.PasswordHash = hasher.HashPassword(user, request.NewPassword);
        await db.Set<AuthSession>().Where(x => x.UserId == user.Id && x.RevokedAt == null).ExecuteUpdateAsync(s => s.SetProperty(x => x.RevokedAt, now), ct);
        await db.SaveChangesAsync(ct);
        await transaction.CommitAsync(ct);
        return NoContent();
    }

    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "User"), HttpPost("profile")]
    public async Task<ActionResult<UserResponse>> UpdateProfile(UpdateProfileRequest request, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.Name)) return BadRequest(new { title = "A name is required." });
        var user = await db.Users.Include(x => x.Roles).SingleAsync(x => x.Id == UserId && x.IsActive, ct);
        user.Name = request.Name.Trim(); await db.SaveChangesAsync(ct);
        return Ok(UserResponse.From(user));
    }
}
