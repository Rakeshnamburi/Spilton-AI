using System.Net;
using System.Text;
using Spilton.Api.Chat;
namespace Spilton.Api.Tests;

public sealed class CompatibleTransportTests
{
    private sealed class FixtureHandler(HttpStatusCode status, string response) : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            => Task.FromResult(new HttpResponseMessage(status) { Content = new StringContent(response, Encoding.UTF8, "text/event-stream") });
    }
    private static CompatibleProvider Provider(HttpStatusCode status, string body) => new(new ModelSettings { BaseUrl = "https://fixture.invalid/v1", ApiKey = "TEST_FIXTURE_NOT_A_REAL_KEY", Model = "fixture-model" }, new HttpClient(new FixtureHandler(status, body)));
    [Fact]
    public async Task Parses_streamed_content_without_exposing_reasoning()
    {
        var provider = Provider(HttpStatusCode.OK, "data: {\"choices\":[{\"delta\":{\"reasoning_content\":\"PRIVATE_REASONING\",\"content\":\"Hello\"}}]}\n\ndata: {\"choices\":[{\"delta\":{\"content\":\" world\"},\"finish_reason\":\"stop\"}]}\n\ndata: [DONE]\n\n");
        var output = ""; await foreach (var chunk in provider.StreamAsync([new("user", "Hi")], default)) output += chunk;
        Assert.Equal("Hello world", output);
    }
    [Theory]
    [InlineData(HttpStatusCode.TooManyRequests, "rate_limit")]
    [InlineData(HttpStatusCode.Unauthorized, "provider_failure")]
    public async Task Provider_HTTP_errors_are_sanitized(HttpStatusCode status, string code)
    {
        var ex = await Assert.ThrowsAsync<ProviderException>(async () => { await foreach (var chunk in Provider(status, "SECRET_RESPONSE_BODY").StreamAsync([], default)) { } });
        Assert.Equal(code, ex.Code); Assert.DoesNotContain("SECRET_RESPONSE_BODY", ex.Message);
    }
    [Fact]
    public async Task Unexpected_end_is_not_success()
    {
        var ex = await Assert.ThrowsAsync<ProviderException>(async () => { await foreach (var chunk in Provider(HttpStatusCode.OK, "data: {\"choices\":[{\"delta\":{\"content\":\"Partial\"}}]}\n\n").StreamAsync([], default)) { } });
        Assert.Equal("connection_interrupted", ex.Code);
    }
}
