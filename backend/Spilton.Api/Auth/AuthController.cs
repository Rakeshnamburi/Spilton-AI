using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Spilton.Api.Data;
namespace Spilton.Api.Auth;

public sealed record RegisterRequest(
    [Required, StringLength(100, MinimumLength = 1)] string Name,
    [Required, EmailAddress, StringLength(254)] string Email,
    [Required, StringLength(128, MinimumLength = 6)] string Password);
public sealed record LoginRequest(
    [Required, EmailAddress, StringLength(254)] string Email,
    [Required, StringLength(128)] string Password);

[ApiController, Route("api/auth")]
public sealed class AuthController(AppDbContext db, IPasswordHasher<User> hasher, SessionService sessions, LoginTimingGuard timing, VerificationDelivery verification) : ControllerBase
{
    [HttpPost("register"), EnableRateLimiting("auth")]
    public async Task<ActionResult<AuthResponse>> Register(RegisterRequest request, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Password))
            return Problem(statusCode: 400, title: "A name and a non-blank password are required.");
        var normalizedEmail = request.Email.Trim().ToUpperInvariant();
        if (await db.Users.AnyAsync(u => u.NormalizedEmail == normalizedEmail, ct))
            return Problem(statusCode: 409, title: "An account with this email already exists.");
        var user = new User { Name = request.Name.Trim(), Email = request.Email.Trim(), NormalizedEmail = normalizedEmail };
        user.PasswordHash = hasher.HashPassword(user, request.Password);
        user.Roles.Add(await db.Roles.SingleAsync(r => r.Id == AppDbContext.UserRoleId, ct));
        db.Users.Add(user);
        try { await db.SaveChangesAsync(ct); }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException { SqlState: PostgresErrorCodes.UniqueViolation })
        { return Problem(statusCode: 409, title: "An account with this email already exists."); }
        Response.Headers.CacheControl = "no-store";
        await verification.Send(user, ct);
        return StatusCode(201, await sessions.Create(user, ct));
    }
    [HttpPost("login"), EnableRateLimiting("auth")]
    public async Task<ActionResult<AuthResponse>> Login(LoginRequest request, CancellationToken ct)
    {
        var normalizedEmail = request.Email.Trim().ToUpperInvariant();
        var user = await db.Users.Include(u => u.Roles).SingleOrDefaultAsync(u => u.NormalizedEmail == normalizedEmail, ct);
        var result = hasher.VerifyHashedPassword(user ?? timing.DummyUser, user?.PasswordHash ?? timing.DummyHash, request.Password);
        if (user is null || !user.IsActive || result == PasswordVerificationResult.Failed)
            return Problem(statusCode: 401, title: "Invalid email or password.");
        if (result == PasswordVerificationResult.SuccessRehashNeeded)
        { user.PasswordHash = hasher.HashPassword(user, request.Password); await db.SaveChangesAsync(ct); }
        Response.Headers.CacheControl = "no-store";
        return Ok(await sessions.Create(user, ct));
    }
    [Authorize(Roles = "User"), HttpGet("me")]
    public async Task<ActionResult<UserResponse>> Me(CancellationToken ct)
    {
        var id = Guid.Parse(User.FindFirst("sub")!.Value);
        var user = await db.Users.AsNoTracking().Include(u => u.Roles).SingleAsync(u => u.Id == id, ct);
        Response.Headers.CacheControl = "no-store";
        return Ok(UserResponse.From(user));
    }
}
