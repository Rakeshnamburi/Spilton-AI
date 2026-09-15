using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Spilton.Api.Auth;
using Spilton.Api.Data;

namespace Spilton.Api.Tests;

public sealed class SessionTests : IClassFixture<ChatFactory>
{
    private readonly ChatFactory factory;
    public SessionTests(ChatFactory factory) => this.factory = factory;
    private async Task<AuthResponse> Register(HttpClient client)
    {
        var response = await client.PostAsJsonAsync("/api/auth/register", new { name = "Session test", email = $"session-{Guid.NewGuid():N}@example.test", password = Guid.NewGuid().ToString("N") });
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        return (await response.Content.ReadFromJsonAsync<AuthResponse>())!;
    }
    [Fact]
    public async Task Refresh_rotates_hash_only_and_reuse_revokes_family_and_access()
    {
        using var client = factory.CreateClient();
        var initial = await Register(client);
        using var scope = factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var stored = await db.Set<RefreshCredential>().SingleAsync(c => c.Hash == SessionService.Hash(initial.RefreshToken!));
        Assert.NotEqual(initial.RefreshToken, stored.Hash);
        var rotated = await client.PostAsJsonAsync("/api/auth/refresh", new { initial.RefreshToken });
        Assert.Equal(HttpStatusCode.OK, rotated.StatusCode);
        var next = (await rotated.Content.ReadFromJsonAsync<AuthResponse>())!;
        Assert.NotEqual(initial.RefreshToken, next.RefreshToken);
        client.DefaultRequestHeaders.Authorization = new("Bearer", next.AccessToken);
        Assert.Equal(HttpStatusCode.OK, (await client.GetAsync("/api/auth/me")).StatusCode);
        Assert.Equal(HttpStatusCode.Unauthorized, (await client.PostAsJsonAsync("/api/auth/refresh", new { initial.RefreshToken })).StatusCode);
        Assert.Equal(HttpStatusCode.Unauthorized, (await client.GetAsync("/api/auth/me")).StatusCode);
        Assert.Equal(HttpStatusCode.Unauthorized, (await client.PostAsJsonAsync("/api/auth/refresh", new { next.RefreshToken })).StatusCode);
    }
    [Fact]
    public async Task Logout_revokes_only_owned_session_and_refresh()
    {
        using var client = factory.CreateClient(); var auth = await Register(client);
        client.DefaultRequestHeaders.Authorization = new("Bearer", auth.AccessToken);
        var items = await client.GetFromJsonAsync<System.Text.Json.JsonElement>("/api/auth/sessions");
        Assert.Equal(1, items.GetArrayLength());
        Assert.Equal(HttpStatusCode.NoContent, (await client.PostAsJsonAsync("/api/auth/logout", new {})).StatusCode);
        Assert.Equal(HttpStatusCode.Unauthorized, (await client.GetAsync("/api/auth/me")).StatusCode);
        Assert.Equal(HttpStatusCode.Unauthorized, (await client.PostAsJsonAsync("/api/auth/refresh", new { auth.RefreshToken })).StatusCode);
    }
    [Fact]
    public async Task Expired_session_cannot_refresh_or_authorize()
    {
        using var client = factory.CreateClient(); var auth = await Register(client);
        using var scope = factory.Services.CreateScope(); var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await db.Set<AuthSession>().Where(s => s.UserId == auth.User.Id).ExecuteUpdateAsync(s => s.SetProperty(x => x.ExpiresAt, DateTimeOffset.UtcNow.AddSeconds(-1)));
        client.DefaultRequestHeaders.Authorization = new("Bearer", auth.AccessToken);
        Assert.Equal(HttpStatusCode.Unauthorized, (await client.GetAsync("/api/auth/me")).StatusCode);
        Assert.Equal(HttpStatusCode.Unauthorized, (await client.PostAsJsonAsync("/api/auth/refresh", new { auth.RefreshToken })).StatusCode);
    }
}
