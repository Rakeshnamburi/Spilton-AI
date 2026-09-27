using System.Net;
using System.Text;
using Microsoft.Extensions.Logging.Abstractions;
using Spilton.Api.Images;

namespace Spilton.Api.Tests;

public sealed class ImageGenerationTests
{
    [Fact]
    public void Configuration_requires_https_key_and_model()
    {
        Assert.False(new ImageGenerationSettings { Provider="OpenAICompatible", BaseUrl="http://example.test/v1", ApiKey="key", Model="image-model" }.IsConfigured);
        Assert.True(new ImageGenerationSettings { Provider="OpenAICompatible", BaseUrl="https://example.test/v1", ApiKey="key", Model="image-model" }.IsConfigured);
    }

    [Fact]
    public async Task Compatible_provider_returns_bounded_png_data_url()
    {
        var bytes = Enumerable.Repeat((byte)7, 256).ToArray();
        var handler = new StubHandler("{\"data\":[{\"b64_json\":\"" + Convert.ToBase64String(bytes) + "\",\"revised_prompt\":\"clean prompt\"}]}");
        var provider = new OpenAiCompatibleImageGenerationProvider(
            new(){Provider="OpenAICompatible",BaseUrl="https://images.example.test/v1",ApiKey="secret",Model="image-model"},
            new StubFactory(handler), NullLogger<OpenAiCompatibleImageGenerationProvider>.Instance);
        var result = await provider.GenerateAsync("draw a plane", "square", default);
        Assert.StartsWith("data:image/png;base64,", result.DataUrl);
        Assert.Equal("clean prompt", result.RevisedPrompt);
        Assert.Contains("/images/generations", handler.LastRequest!.RequestUri!.AbsoluteUri);
        Assert.Equal("Bearer", handler.LastRequest.Headers.Authorization!.Scheme);
    }

    private sealed class StubFactory(HttpMessageHandler handler):IHttpClientFactory
    { public HttpClient CreateClient(string name)=>new(handler,false); }
    private sealed class StubHandler(string json):HttpMessageHandler
    {
        public HttpRequestMessage? LastRequest { get; private set; }
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,CancellationToken cancellationToken)
        {
            LastRequest=request;
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK){Content=new StringContent(json,Encoding.UTF8,"application/json")});
        }
    }
}
