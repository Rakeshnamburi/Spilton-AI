using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Spilton.Api.Data;

namespace Spilton.Api.Auth;

public sealed class AuthSession
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset ExpiresAt { get; set; }
    public DateTimeOffset? RevokedAt { get; set; }
    public DateTimeOffset LastUsedAt { get; set; } = DateTimeOffset.UtcNow;
}
public sealed class RefreshCredential
{
    public string Hash { get; set; } = "";
    public Guid SessionId { get; set; }
    public DateTimeOffset? UsedAt { get; set; }
}
public static class SessionModel
{
    public static void Configure(ModelBuilder model)
    {
        var sessions = model.Entity<AuthSession>();
        sessions.HasKey(s => s.Id);
        sessions.HasOne<User>().WithMany().HasForeignKey(s => s.UserId).OnDelete(DeleteBehavior.Cascade);
        sessions.HasIndex(s => new { s.UserId, s.ExpiresAt });
        var credentials = model.Entity<RefreshCredential>();
        credentials.HasKey(c => c.Hash);
        credentials.Property(c => c.Hash).HasMaxLength(64);
        credentials.HasOne<AuthSession>().WithMany().HasForeignKey(c => c.SessionId).OnDelete(DeleteBehavior.Cascade);
    }
}
public sealed class SessionService(AppDbContext db, TokenService tokens, TimeProvider clock)
{
    public static string Hash(string token) => Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(token)));
    private static string NewToken() => Convert.ToHexString(RandomNumberGenerator.GetBytes(48));
    public async Task<AuthResponse> Create(User user, CancellationToken ct)
    {
        var now = clock.GetUtcNow();
        var session = new AuthSession { UserId = user.Id, CreatedAt = now, LastUsedAt = now, ExpiresAt = now.AddDays(7) };
        var refresh = NewToken();
        db.Add(session); db.Add(new RefreshCredential { SessionId = session.Id, Hash = Hash(refresh) });
        await db.SaveChangesAsync(ct);
        return tokens.Create(user, session.Id) with { RefreshToken = refresh, RefreshExpiresAt = session.ExpiresAt };
    }
    public async Task<AuthResponse?> Rotate(string refresh, CancellationToken ct)
    {
        if (refresh.Length != 96 || !refresh.All(Uri.IsHexDigit)) return null;
        var hash = Hash(refresh);
        var credential = await db.Set<RefreshCredential>().AsNoTracking().SingleOrDefaultAsync(x => x.Hash == hash, ct);
        if (credential is null) return null;
        await using var tx = await db.Database.BeginTransactionAsync(ct);
        // Lock the family, not only one credential: concurrent rotations/logout serialize.
        var session = await db.Set<AuthSession>().FromSqlInterpolated($"SELECT * FROM \"AuthSession\" WHERE \"Id\" = {credential.SessionId} FOR UPDATE").SingleAsync(ct);
        var now = clock.GetUtcNow();
        var stored = await db.Set<RefreshCredential>().SingleAsync(x => x.Hash == hash, ct);
        if (stored.UsedAt is not null)
        {
            session.RevokedAt ??= now;
            await db.SaveChangesAsync(ct); await tx.CommitAsync(ct);
            return null;
        }
        if (session.RevokedAt is not null || session.ExpiresAt <= now) return null;
        var user = await db.Users.Include(x => x.Roles).SingleAsync(x => x.Id == session.UserId, ct);
        if (!user.IsActive) return null;
        stored.UsedAt = now; session.LastUsedAt = now;
        var next = NewToken();
        db.Add(new RefreshCredential { SessionId = session.Id, Hash = Hash(next) });
        await db.SaveChangesAsync(ct); await tx.CommitAsync(ct);
        return tokens.Create(user, session.Id) with { RefreshToken = next, RefreshExpiresAt = session.ExpiresAt };
    }
    public async Task RevokeRefresh(string refresh, CancellationToken ct)
    {
        if (refresh.Length != 96 || !refresh.All(Uri.IsHexDigit)) return;
        var hash = Hash(refresh);
        var id = await db.Set<RefreshCredential>().Where(c => c.Hash == hash).Select(c => (Guid?)c.SessionId).SingleOrDefaultAsync(ct);
        if (id is not null) await db.Set<AuthSession>().Where(s => s.Id == id)
            .ExecuteUpdateAsync(s => s.SetProperty(x => x.RevokedAt, DateTimeOffset.UtcNow), ct);
    }
}
public sealed record RefreshRequest([Required, StringLength(96, MinimumLength = 96)] string RefreshToken);

[ApiController, Route("api/auth"), ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
public sealed class SessionsController(AppDbContext db, SessionService sessions) : ControllerBase
{
    [HttpPost("revoke-refresh"), EnableRateLimiting("auth")]
    public async Task<IActionResult> RevokeRefresh(RefreshRequest request, CancellationToken ct)
    {
        await sessions.RevokeRefresh(request.RefreshToken, ct);
        return NoContent();
    }
    [HttpPost("refresh"), EnableRateLimiting("auth")]
    public async Task<IActionResult> Refresh(RefreshRequest request, CancellationToken ct)
    {
        var response = await sessions.Rotate(request.RefreshToken, ct);
        return response is null ? Unauthorized(new { title = "Session expired. Please sign in." }) : Ok(response);
    }
    [Authorize, HttpPost("logout")]
    public async Task<IActionResult> Logout(CancellationToken ct)
    {
        if (Guid.TryParse(User.FindFirst("sid")?.Value, out var sid))
            await db.Set<AuthSession>().Where(s => s.Id == sid && s.UserId == Guid.Parse(User.FindFirst("sub")!.Value))
                .ExecuteUpdateAsync(s => s.SetProperty(x => x.RevokedAt, DateTimeOffset.UtcNow), ct);
        return NoContent();
    }
    [Authorize, HttpPost("logout-all")]
    public async Task<IActionResult> LogoutAll(CancellationToken ct)
    {
        var user = Guid.Parse(User.FindFirst("sub")!.Value);
        await db.Set<AuthSession>().Where(s => s.UserId == user && s.RevokedAt == null)
            .ExecuteUpdateAsync(s => s.SetProperty(x => x.RevokedAt, DateTimeOffset.UtcNow), ct);
        return NoContent();
    }
    [Authorize, HttpGet("sessions")]
    public async Task<IActionResult> List(CancellationToken ct)
    {
        var user = Guid.Parse(User.FindFirst("sub")!.Value);
        return Ok(await db.Set<AuthSession>().AsNoTracking().Where(s => s.UserId == user && s.RevokedAt == null && s.ExpiresAt > DateTimeOffset.UtcNow)
            .OrderByDescending(s => s.CreatedAt).Select(s => new { s.Id, s.CreatedAt, s.LastUsedAt, s.ExpiresAt }).Take(50).ToListAsync(ct));
    }
}
