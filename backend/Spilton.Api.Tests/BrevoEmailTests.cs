using System.Net;
using System.Text.Json;
using Spilton.Api.Auth;

namespace Spilton.Api.Tests;

public class BrevoEmailTests
{
    private sealed class Transport(HttpStatusCode status) : HttpMessageHandler, IHttpClientFactory
    {
        public string? Body;
        public string? Key;
        public Uri? Uri;
        public HttpClient CreateClient(string name) => new(this, false);
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct)
        {
            Uri = request.RequestUri;
            Key = request.Headers.GetValues("api-key").Single();
            Body = await request.Content!.ReadAsStringAsync(ct);
            return new HttpResponseMessage(status) { Content = new StringContent("sensitive-provider-detail") };
        }
    }

    private static EmailSettings Settings() => new() { Provider = "Brevo", BrevoApiKey = "test-key", FromAddress = "sender@example.test", FromName = "Spilton AI" };

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task Sends_reset_and_verification_through_https(bool verification)
    {
        var transport = new Transport(HttpStatusCode.Created);
        var sender = new BrevoAccountEmailSender(Settings(), transport);
        if (verification) await sender.SendEmailVerificationCode("member@example.test", "Member", "123456", default);
        else await sender.SendPasswordResetCode("member@example.test", "Member", "123456", default);
        Assert.Equal("https://api.brevo.com/v3/smtp/email", transport.Uri!.ToString());
        Assert.Equal("test-key", transport.Key);
        using var json = JsonDocument.Parse(transport.Body!);
        Assert.Equal("member@example.test", json.RootElement.GetProperty("to")[0].GetProperty("email").GetString());
        Assert.Contains("123456", json.RootElement.GetProperty("textContent").GetString());
        Assert.Contains(verification ? "Verify" : "reset", json.RootElement.GetProperty("subject").GetString());
    }

    [Fact]
    public async Task Rejects_provider_failure_without_exposing_response_body()
    {
        var sender = new BrevoAccountEmailSender(Settings(), new Transport(HttpStatusCode.Unauthorized));
        var error = await Assert.ThrowsAsync<HttpRequestException>(() => sender.SendPasswordResetCode("member@example.test", "Member", "123456", default));
        Assert.Equal(HttpStatusCode.Unauthorized, error.StatusCode);
        Assert.DoesNotContain("sensitive", error.Message);
        Assert.DoesNotContain("123456", error.Message);
    }

    [Fact]
    public async Task Missing_key_disables_delivery()
    {
        var settings = Settings(); settings.BrevoApiKey = "";
        var transport = new Transport(HttpStatusCode.Created);
        var sender = new BrevoAccountEmailSender(settings, transport);
        Assert.False(sender.Available);
        await Assert.ThrowsAsync<InvalidOperationException>(() => sender.SendPasswordResetCode("member@example.test", "Member", "123456", default));
        Assert.Null(transport.Body);
    }
}
