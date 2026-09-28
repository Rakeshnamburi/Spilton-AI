using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Spilton.Api.Data;

namespace Spilton.Api.Auth;

public sealed class GoogleSettings
{
    public string ClientId { get; set; } = "";
    public string ClientSecret { get; set; } = "";
    public string RedirectUri { get; set; } = "";
    public string FrontendOrigin { get; set; } = "";
    public bool IsConfigured => !string.IsNullOrWhiteSpace(ClientId) && !string.IsNullOrWhiteSpace(ClientSecret)
        && Uri.TryCreate(RedirectUri, UriKind.Absolute, out var redirect) && redirect.Scheme == Uri.UriSchemeHttps
        && Uri.TryCreate(FrontendOrigin, UriKind.Absolute, out var frontend) && frontend.Scheme == Uri.UriSchemeHttps;
}

public sealed class GoogleLoginChallenge
{
    public string StateHash { get; set; } = "";
    public string CodeVerifier { get; set; } = "";
    public string Nonce { get; set; } = "";
    public DateTimeOffset ExpiresAt { get; set; }
    public DateTimeOffset? UsedAt { get; set; }
}

public sealed class GoogleLoginTicket
{
    public string TicketHash { get; set; } = "";
    public Guid UserId { get; set; }
    public DateTimeOffset ExpiresAt { get; set; }
    public DateTimeOffset? UsedAt { get; set; }
}

public static class GoogleOAuthModel
{
    public static void Configure(ModelBuilder model)
    {
        var challenges = model.Entity<GoogleLoginChallenge>();
        challenges.HasKey(x => x.StateHash); challenges.Property(x => x.StateHash).HasMaxLength(64);
        challenges.Property(x => x.CodeVerifier).HasMaxLength(128); challenges.Property(x => x.Nonce).HasMaxLength(96);
        challenges.HasIndex(x => x.ExpiresAt);
        var tickets = model.Entity<GoogleLoginTicket>();
        tickets.HasKey(x => x.TicketHash); tickets.Property(x => x.TicketHash).HasMaxLength(64);
        tickets.HasOne<User>().WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
        tickets.HasIndex(x => x.ExpiresAt);
    }
}

public sealed record GoogleTicketRequest([Required, StringLength(128, MinimumLength = 64)] string Ticket);

[ApiController, Route("api/auth/google"), ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
public sealed class GoogleOAuthController(
    AppDbContext db,
    GoogleSettings settings,
    IHttpClientFactory clients,
    IPasswordHasher<User> hasher,
    SessionService sessions,
    TimeProvider clock,
    ILogger<GoogleOAuthController> logger) : ControllerBase
{
    private const string StateCookie = "spilton_google_state";
    private static string NewToken(int bytes = 32) => Convert.ToHexString(RandomNumberGenerator.GetBytes(bytes));
    private static string Hash(string value) => Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(value)));
    private string Frontend(string path)
    {
        var root = settings.FrontendOrigin.TrimEnd('/');
        return root + path;
    }

    [HttpGet("start"), EnableRateLimiting("auth")]
    public async Task<IActionResult> Start(CancellationToken ct)
    {
        if (!settings.IsConfigured) return Redirect(Frontend("/login?oauthError=Google+sign-in+is+not+configured."));
        var now = clock.GetUtcNow();
        await db.Set<GoogleLoginChallenge>().Where(x => x.ExpiresAt <= now || x.UsedAt != null).ExecuteDeleteAsync(ct);
        var state = NewToken(); var nonce = NewToken(); var verifier = NewToken(48);
        var challenge = Convert.ToBase64String(SHA256.HashData(Encoding.ASCII.GetBytes(verifier))).TrimEnd('=').Replace('+', '-').Replace('/', '_');
        db.Add(new GoogleLoginChallenge { StateHash = Hash(state), Nonce = nonce, CodeVerifier = verifier, ExpiresAt = now.AddMinutes(10) });
        await db.SaveChangesAsync(ct);
        Response.Cookies.Append(StateCookie, state, new CookieOptions
        {
            HttpOnly = true, Secure = true, SameSite = SameSiteMode.Lax,
            MaxAge = TimeSpan.FromMinutes(10), Path = "/api/auth/google/callback"
        });
        var query = new Dictionary<string, string?>
        {
            ["client_id"] = settings.ClientId, ["redirect_uri"] = settings.RedirectUri, ["response_type"] = "code",
            ["scope"] = "openid email profile", ["state"] = state, ["nonce"] = nonce,
            ["code_challenge"] = challenge, ["code_challenge_method"] = "S256", ["prompt"] = "select_account"
        };
        return Redirect(Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString("https://accounts.google.com/o/oauth2/v2/auth", query));
    }

    [HttpGet("callback"), EnableRateLimiting("auth")]
    public async Task<IActionResult> Callback([FromQuery] string? code, [FromQuery] string? state, [FromQuery] string? error, CancellationToken ct)
    {
        var cookieState = Request.Cookies[StateCookie];
        Response.Cookies.Delete(StateCookie, new CookieOptions { Secure = true, SameSite = SameSiteMode.Lax, Path = "/api/auth/google/callback" });
        if (!settings.IsConfigured || !string.IsNullOrWhiteSpace(error) || string.IsNullOrWhiteSpace(code) || code.Length > 4096
            || string.IsNullOrWhiteSpace(state) || state.Length != 64 || string.IsNullOrWhiteSpace(cookieState)
            || !CryptographicOperations.FixedTimeEquals(SHA256.HashData(Encoding.UTF8.GetBytes(state)), SHA256.HashData(Encoding.UTF8.GetBytes(cookieState))))
            return Redirect(Frontend("/login?oauthError=Google+sign-in+was+cancelled+or+could+not+start."));
        var now = clock.GetUtcNow();
        var challenge = await db.Set<GoogleLoginChallenge>().SingleOrDefaultAsync(x => x.StateHash == Hash(state) && x.UsedAt == null && x.ExpiresAt > now, ct);
        if (challenge is null) return Redirect(Frontend("/login?oauthError=Google+sign-in+expired.+Please+try+again."));
        var consumed = await db.Set<GoogleLoginChallenge>().Where(x => x.StateHash == challenge.StateHash && x.UsedAt == null && x.ExpiresAt > now)
            .ExecuteUpdateAsync(update => update.SetProperty(x => x.UsedAt, now), ct);
        if (consumed != 1) return Redirect(Frontend("/login?oauthError=Google+sign-in+expired.+Please+try+again."));
        try
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, "https://oauth2.googleapis.com/token")
            {
                Content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    ["code"] = code, ["client_id"] = settings.ClientId, ["client_secret"] = settings.ClientSecret,
                    ["redirect_uri"] = settings.RedirectUri, ["grant_type"] = "authorization_code", ["code_verifier"] = challenge.CodeVerifier
                })
            };
            using var response = await clients.CreateClient("google-oauth").SendAsync(request, ct);
            if (!response.IsSuccessStatusCode) throw new InvalidOperationException($"Token exchange failed with HTTP {(int)response.StatusCode}.");
            using var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync(ct));
            if (!json.RootElement.TryGetProperty("id_token", out var idTokenElement)) throw new InvalidOperationException("Google did not return an identity token.");
            var payload = await GoogleJsonWebSignature.ValidateAsync(idTokenElement.GetString(), new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = [settings.ClientId]
            });
            if (!payload.EmailVerified || string.IsNullOrWhiteSpace(payload.Email) || string.IsNullOrWhiteSpace(payload.Subject))
                throw new InvalidOperationException("Google email is not verified.");
            if (!string.Equals(payload.Nonce, challenge.Nonce, StringComparison.Ordinal))
                throw new InvalidOperationException("Google sign-in nonce is invalid.");
            var normalized = payload.Email.Trim().ToUpperInvariant();
            var user = await db.Users.Include(x => x.Roles).SingleOrDefaultAsync(x => x.NormalizedEmail == normalized, ct);
            if (user is not null && !user.IsActive) throw new InvalidOperationException("This account is disabled.");
            if (user is null)
            {
                var displayName = string.IsNullOrWhiteSpace(payload.Name) ? payload.Email.Split('@')[0] : payload.Name.Trim();
                user = new User { Name = displayName[..Math.Min(displayName.Length, 100)], Email = payload.Email.Trim(), NormalizedEmail = normalized, EmailVerifiedAt = now };
                user.PasswordHash = hasher.HashPassword(user, NewToken(48));
                user.Roles.Add(await db.Roles.SingleAsync(r => r.Id == AppDbContext.UserRoleId, ct));
                db.Users.Add(user);
            }
            else if (user.EmailVerifiedAt is null) user.EmailVerifiedAt = now;
            var ticket = NewToken(48);
            db.Add(new GoogleLoginTicket { TicketHash = Hash(ticket), UserId = user.Id, ExpiresAt = now.AddMinutes(2) });
            await db.SaveChangesAsync(ct);
            return Redirect(Frontend($"/api/auth/google/complete?ticket={Uri.EscapeDataString(ticket)}"));
        }
        catch (Exception ex)
        {
            logger.LogWarning("Google sign-in failed. ErrorType={ErrorType} Message={Message}", ex.GetType().Name, ex.Message);
            return Redirect(Frontend("/login?oauthError=Google+sign-in+failed.+Check+the+OAuth+settings+and+try+again."));
        }
    }

    [HttpPost("exchange"), EnableRateLimiting("auth")]
    public async Task<ActionResult<AuthResponse>> Exchange(GoogleTicketRequest request, CancellationToken ct)
    {
        var now = clock.GetUtcNow(); var hash = Hash(request.Ticket);
        var ticket = await db.Set<GoogleLoginTicket>().SingleOrDefaultAsync(x => x.TicketHash == hash && x.UsedAt == null && x.ExpiresAt > now, ct);
        if (ticket is null) return Problem(statusCode: 400, title: "Google sign-in session is invalid or expired.");
        var consumed = await db.Set<GoogleLoginTicket>().Where(x => x.TicketHash == hash && x.UsedAt == null && x.ExpiresAt > now)
            .ExecuteUpdateAsync(update => update.SetProperty(x => x.UsedAt, now), ct);
        if (consumed != 1) return Problem(statusCode: 400, title: "Google sign-in session is invalid or expired.");
        var user = await db.Users.Include(x => x.Roles).SingleAsync(x => x.Id == ticket.UserId && x.IsActive, ct);
        return Ok(await sessions.Create(user, ct));
    }
}
