using System.Net;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Spilton.Api.Auth;
using Spilton.Api.Chat;

namespace Spilton.Api.Tests;

public sealed class ChatFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");
        builder.ConfigureServices(services => {
            services.RemoveAll<ModelSettings>();
            services.AddSingleton(new ModelSettings { DevelopmentEnabled = true, TimeoutSeconds = 5 });
            services.RemoveAll<IModelProviderResolver>();
            services.AddScoped<IModelProviderResolver, TestResolver>();
            services.RemoveAll<IAccountEmailSender>();
            services.AddSingleton<TestAccountEmailSender>();
            services.AddSingleton<IAccountEmailSender>(s => s.GetRequiredService<TestAccountEmailSender>());
        });
    }
}
public sealed class TestAccountEmailSender : IAccountEmailSender
{
    private readonly System.Collections.Concurrent.ConcurrentDictionary<string,string> codes=new(StringComparer.OrdinalIgnoreCase);
    public bool Available => true;
    public Task SendPasswordResetCode(string email,string name,string code,CancellationToken ct){codes[email]=code;return Task.CompletedTask;}
    public string CodeFor(string email)=>codes[email];
}
// Test-only fault injection: these providers cannot be selected in the running application.
public sealed class TestResolver(ModelProviderFactory real) : IModelProviderResolver
{
    public IReadOnlyList<ModelInfo> Available => real.Available;
    public string DefaultId => real.DefaultId;
    public IModelProvider Resolve(string? id) => id is "failure" or "empty" or "timeout" or "partial" or "oversize" ? new FaultProvider(id) : real.Resolve(id);
}
public sealed class FaultProvider(string scenario) : IModelProvider
{
    public ModelInfo Info => new(scenario, "Test fixture", "TestFixture", scenario, true);
    public async IAsyncEnumerable<string> StreamAsync(IReadOnlyList<ModelMessage> messages, [EnumeratorCancellation] CancellationToken ct)
    {
        await Task.Yield();
        if (scenario == "empty") yield break;
        if (scenario == "timeout") { await Task.Delay(20000, ct); yield break; }
        if (scenario == "partial") yield return "Partial answer saved";
        if (scenario == "oversize") { yield return new string('a', 32001); yield break; }
        throw new HttpRequestException("PRIVATE_KEY_MUST_NOT_LEAK");
    }
}
public sealed class ChatFailureTests : IClassFixture<ChatFactory>
{
    private readonly ChatFactory factory;
    public ChatFailureTests(ChatFactory factory) => this.factory = factory;
    [Theory]
    [InlineData("failure", "provider_failure")]
    [InlineData("empty", "empty_response")]
    [InlineData("timeout", "timeout")]
    [InlineData("partial", "provider_failure")]
    [InlineData("oversize", "response_limit")]
    public async Task Provider_failure_is_safe_and_persisted(string scenario, string code)
    {
        using var client = factory.CreateClient();
        var auth = await client.PostAsJsonAsync("/api/auth/register", new { name = "Fault test", email = $"fault-{Guid.NewGuid():N}@example.test", password = Guid.NewGuid().ToString("N") });
        Assert.Equal(HttpStatusCode.Created, auth.StatusCode);
        var token = (await auth.Content.ReadFromJsonAsync<JsonElement>()).GetProperty("accessToken").GetString();
        client.DefaultRequestHeaders.Authorization = new("Bearer", token);
        var created = await client.PostAsJsonAsync("/api/conversations", new { });
        var id = (await created.Content.ReadFromJsonAsync<JsonElement>()).GetProperty("id").GetString();
        var response = await client.PostAsJsonAsync($"/api/conversations/{id}/messages", new { content = "Test failure recovery", model = scenario });
        var body = await response.Content.ReadAsStringAsync();
        Assert.Contains($"\"code\":\"{code}\"", body);
        Assert.DoesNotContain("PRIVATE_KEY_MUST_NOT_LEAK", body);
        Assert.Contains("event: done", body);
        var detail = await client.GetFromJsonAsync<JsonElement>($"/api/conversations/{id}");
        var messages = detail.GetProperty("messages");
        Assert.Equal(2, messages.GetArrayLength()); Assert.Equal("failed", messages[1].GetProperty("status").GetString());
        if (scenario == "partial") Assert.Equal("Partial answer saved", messages[1].GetProperty("content").GetString());
        Assert.Equal(HttpStatusCode.NoContent, (await client.DeleteAsync($"/api/conversations/{id}")).StatusCode);
    }
    [Fact]
    public void Context_is_bounded_and_excludes_failed_answers_and_custom_system_messages()
    {
        var history = Enumerable.Range(0, 80).Select(i => new Message { Role = i % 2 == 0 ? "USER" : "ASSISTANT", Content = new string('a', 1500), Sequence = i }).ToList();
        history.Add(new Message { Role = "ASSISTANT", Content = "FAILED_MARKER", Status = "failed" });
        history.Add(new Message { Role = "SYSTEM", Content = "UNTRUSTED_SYSTEM" });
        var context = new ContextBuilder().Build(history);
        Assert.True(context.Count <= 21); Assert.True(context.Skip(1).Sum(m => m.Content.Length) <= 16000);
        Assert.Equal("system", context[0].Role); Assert.Equal("user", context[1].Role);
        Assert.DoesNotContain(context, m => m.Content.Contains("FAILED_MARKER") || m.Content.Contains("UNTRUSTED_SYSTEM"));
    }
    [Fact]
    public async Task Expired_JWT_is_rejected()
    {
        using var client = factory.CreateClient();
        var settings = factory.Services.GetRequiredService<Spilton.Api.Auth.JwtSettings>();
        var token = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(settings.Issuer, settings.Audience,
            [new System.Security.Claims.Claim("sub", Guid.NewGuid().ToString())], DateTime.UtcNow.AddHours(-2), DateTime.UtcNow.AddHours(-1),
            new Microsoft.IdentityModel.Tokens.SigningCredentials(new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(settings.Secret)), Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256));
        client.DefaultRequestHeaders.Authorization = new("Bearer", new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler().WriteToken(token));
        Assert.Equal(HttpStatusCode.Unauthorized, (await client.GetAsync("/api/conversations")).StatusCode);
    }
    [Fact]
    public void Incomplete_provider_configuration_is_not_advertised()
    {
        var environment = factory.Services.GetRequiredService<IWebHostEnvironment>();
        var clients = factory.Services.GetRequiredService<IHttpClientFactory>();
        var resolver = new ModelProviderFactory(new ModelSettings { DevelopmentEnabled = false, BaseUrl = "https://example.invalid/v1", Model = "unavailable-model" }, environment, clients);
        Assert.Empty(resolver.Available);
        Assert.Throws<ProviderException>(() => resolver.Resolve("compatible"));
    }
    [Fact]
    public async Task Failed_regeneration_keeps_original_then_success_supersedes_all_earlier_versions()
    {
        using var client = factory.CreateClient();
        var auth = await client.PostAsJsonAsync("/api/auth/register", new { name = "Version test", email = $"versions-{Guid.NewGuid():N}@example.test", password = Guid.NewGuid().ToString("N") });
        var token = (await auth.Content.ReadFromJsonAsync<JsonElement>()).GetProperty("accessToken").GetString();
        client.DefaultRequestHeaders.Authorization = new("Bearer", token);
        var created = await client.PostAsJsonAsync("/api/conversations", new { });
        var id = (await created.Content.ReadFromJsonAsync<JsonElement>()).GetProperty("id").GetString();
        await client.PostAsJsonAsync($"/api/conversations/{id}/messages", new { content = "Keep the original answer", model = "development" });
        var original = (await client.GetFromJsonAsync<JsonElement>($"/api/conversations/{id}")).GetProperty("messages")[1].GetProperty("id").GetString();
        await client.PostAsJsonAsync($"/api/conversations/{id}/regenerate", new { model = "failure" });
        var failed = (await client.GetFromJsonAsync<JsonElement>($"/api/conversations/{id}")).GetProperty("messages");
        Assert.Equal(3, failed.GetArrayLength()); Assert.Equal(original, failed[1].GetProperty("id").GetString());
        await client.PostAsJsonAsync($"/api/conversations/{id}/regenerate", new { model = "development" });
        var completed = (await client.GetFromJsonAsync<JsonElement>($"/api/conversations/{id}")).GetProperty("messages");
        Assert.Equal(2, completed.GetArrayLength()); Assert.Equal("completed", completed[1].GetProperty("status").GetString());
        Assert.NotEqual(original, completed[1].GetProperty("id").GetString());
        await client.DeleteAsync($"/api/conversations/{id}");
    }
}
