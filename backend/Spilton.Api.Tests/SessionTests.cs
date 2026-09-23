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
    public async Task Registration_sends_a_verification_code_without_a_separate_request()
    {
        using var client = factory.CreateClient();
        var auth = await Register(client);
        var code = factory.Services.GetRequiredService<TestAccountEmailSender>().CodeFor(auth.User.Email);
        Assert.Matches("^[0-9]{6}$", code);
        client.DefaultRequestHeaders.Authorization = new("Bearer", auth.AccessToken);
        var response = await client.PostAsJsonAsync("/api/auth/verify-email", new { code });
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull((await response.Content.ReadFromJsonAsync<UserResponse>())!.EmailVerifiedAt);
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
    [Fact]
    public async Task Password_reset_code_is_single_use_and_revokes_existing_sessions()
    {
        using var client=factory.CreateClient();var email=$"reset-{Guid.NewGuid():N}@example.test";const string oldPassword="old-password";const string newPassword="new-password";
        var registered=await client.PostAsJsonAsync("/api/auth/register",new{name="Reset user",email,password=oldPassword});
        Assert.Equal(HttpStatusCode.Created,registered.StatusCode);var auth=(await registered.Content.ReadFromJsonAsync<AuthResponse>())!;
        Assert.Equal(HttpStatusCode.Accepted,(await client.PostAsJsonAsync("/api/auth/forgot-password",new{email})).StatusCode);
        var code=factory.Services.GetRequiredService<TestAccountEmailSender>().CodeFor(email);
        var verified=await client.PostAsJsonAsync("/api/auth/verify-reset",new{email,code});Assert.Equal(HttpStatusCode.OK,verified.StatusCode);
        var resetToken=(await verified.Content.ReadFromJsonAsync<System.Text.Json.JsonElement>()).GetProperty("resetToken").GetString();
        Assert.Equal(HttpStatusCode.NoContent,(await client.PostAsJsonAsync("/api/auth/reset-password",new{resetToken,newPassword})).StatusCode);
        client.DefaultRequestHeaders.Authorization=new("Bearer",auth.AccessToken);Assert.Equal(HttpStatusCode.Unauthorized,(await client.GetAsync("/api/auth/me")).StatusCode);
        client.DefaultRequestHeaders.Authorization=null;
        Assert.Equal(HttpStatusCode.Unauthorized,(await client.PostAsJsonAsync("/api/auth/login",new{email,password=oldPassword})).StatusCode);
        Assert.Equal(HttpStatusCode.OK,(await client.PostAsJsonAsync("/api/auth/login",new{email,password=newPassword})).StatusCode);
        Assert.Equal(HttpStatusCode.BadRequest,(await client.PostAsJsonAsync("/api/auth/reset-password",new{resetToken,newPassword="another-password"})).StatusCode);
    }
    [Fact]
    public async Task Signed_in_user_can_update_profile_name()
    {
        using var client=factory.CreateClient();var auth=await Register(client);client.DefaultRequestHeaders.Authorization=new("Bearer",auth.AccessToken);
        var response=await client.PostAsJsonAsync("/api/auth/profile",new{name="Updated profile"});Assert.Equal(HttpStatusCode.OK,response.StatusCode);
        var user=await response.Content.ReadFromJsonAsync<UserResponse>();Assert.Equal("Updated profile",user!.Name);Assert.Equal(auth.User.Email,user.Email);
    }
    [Fact]
    public async Task Signed_in_user_can_verify_email_with_single_use_code()
    {
        using var client=factory.CreateClient();var auth=await Register(client);client.DefaultRequestHeaders.Authorization=new("Bearer",auth.AccessToken);
        Assert.Null(auth.User.EmailVerifiedAt);
        Assert.Equal(HttpStatusCode.Accepted,(await client.PostAsJsonAsync("/api/auth/request-email-verification",new{})).StatusCode);
        var code=factory.Services.GetRequiredService<TestAccountEmailSender>().CodeFor(auth.User.Email);
        Assert.Equal(HttpStatusCode.BadRequest,(await client.PostAsJsonAsync("/api/auth/verify-email",new{code="000000"})).StatusCode);
        var response=await client.PostAsJsonAsync("/api/auth/verify-email",new{code});Assert.Equal(HttpStatusCode.OK,response.StatusCode);
        var user=await response.Content.ReadFromJsonAsync<UserResponse>();Assert.NotNull(user!.EmailVerifiedAt);
        var again=await client.PostAsJsonAsync("/api/auth/verify-email",new{code});Assert.Equal(HttpStatusCode.OK,again.StatusCode);
    }
}
