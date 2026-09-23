using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Spilton.Api.Data;

namespace Spilton.Api.Auth;

public sealed class VerificationDelivery(AppDbContext db, IAccountEmailSender email, TimeProvider clock, ILogger<VerificationDelivery> logger)
{
    public async Task<bool> Send(User user, CancellationToken ct)
    {
        if (!email.Available) return false;
        var now = clock.GetUtcNow();
        await db.Set<AccountChallenge>().Where(x => x.UserId == user.Id && x.Purpose == "EMAIL_VERIFICATION" && x.UsedAt == null)
            .ExecuteUpdateAsync(s => s.SetProperty(x => x.UsedAt, now), ct);
        var code = RandomNumberGenerator.GetInt32(0, 1_000_000).ToString("D6");
        var salt = Convert.ToHexString(RandomNumberGenerator.GetBytes(16));
        var challenge = new AccountChallenge { UserId = user.Id, Purpose = "EMAIL_VERIFICATION", CodeSalt = salt,
            CodeHash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(salt + ":" + code))), CreatedAt = now, ExpiresAt = now.AddMinutes(10) };
        db.Add(challenge);
        await db.SaveChangesAsync(ct);
        try { await email.SendEmailVerificationCode(user.Email, user.Name, code, ct); return true; }
        catch (Exception ex)
        {
            await db.Set<AccountChallenge>().Where(x => x.Id == challenge.Id).ExecuteUpdateAsync(s => s.SetProperty(x => x.UsedAt, now), CancellationToken.None);
            logger.LogWarning("Verification delivery failed. ErrorType={ErrorType} ProviderStatus={ProviderStatus}", ex.GetType().Name, (ex as HttpRequestException)?.StatusCode);
            return false;
        }
    }
}
