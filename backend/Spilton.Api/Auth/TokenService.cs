using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Spilton.Api.Data;
namespace Spilton.Api.Auth;

public sealed class JwtSettings
{
    public string Secret { get; set; } = "";
    public string Issuer { get; set; } = "";
    public string Audience { get; set; } = "";
    public int ExpiryMinutes { get; set; } = 30;
}
public sealed record UserResponse(Guid Id, string Name, string Email, string[] Roles)
{
    public static UserResponse From(User user) => new(user.Id, user.Name, user.Email, user.Roles.Select(r => r.Name).ToArray());
}
public sealed record AuthResponse(string AccessToken, DateTimeOffset ExpiresAt, UserResponse User)
{
    public string? RefreshToken { get; init; }
    public DateTimeOffset? RefreshExpiresAt { get; init; }
}
public sealed class TokenService(JwtSettings settings)
{
    public AuthResponse Create(User user, Guid? sessionId = null)
    {
        var now = DateTimeOffset.UtcNow;
        var expires = now.AddMinutes(settings.ExpiryMinutes);
        var claims = new List<Claim> { new("sub", user.Id.ToString()), new("name", user.Name), new("jti", Guid.NewGuid().ToString()) };
        claims.AddRange(user.Roles.Select(role => new Claim("role", role.Name)));
        if (sessionId is not null) claims.Add(new("sid", sessionId.ToString()!));
        var token = new JwtSecurityToken(settings.Issuer, settings.Audience, claims, now.UtcDateTime, expires.UtcDateTime,
            new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.Secret)), SecurityAlgorithms.HmacSha256));
        return new(new JwtSecurityTokenHandler().WriteToken(token), expires, UserResponse.From(user));
    }
}
// Unknown emails still perform password verification to reduce timing differences.
public sealed class LoginTimingGuard
{
    public User DummyUser { get; } = new();
    public string DummyHash { get; }
    public LoginTimingGuard(IOptions<PasswordHasherOptions> options)
        => DummyHash = new PasswordHasher<User>(options).HashPassword(DummyUser, Guid.NewGuid().ToString("N"));
}
