using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Spilton.Api.Images;

public sealed class ImageGenerationSettings
{
    public string Provider { get; set; } = "Disabled";
    public string BaseUrl { get; set; } = "";
    public string ApiKey { get; set; } = "";
    public string Model { get; set; } = "";
    public int TimeoutSeconds { get; set; } = 120;
    public bool IsConfigured => Provider.Equals("OpenAICompatible", StringComparison.OrdinalIgnoreCase)
        && Uri.TryCreate(BaseUrl, UriKind.Absolute, out var uri) && uri.Scheme == Uri.UriSchemeHttps
        && !string.IsNullOrWhiteSpace(ApiKey) && !string.IsNullOrWhiteSpace(Model)
        && TimeoutSeconds is >= 10 and <= 180;
}

public sealed record ImageGenerationCapability(bool Configured, string Status, string Provider, string? Model);
public sealed record GeneratedImage(string DataUrl, string MimeType, string Provider, string Model, string RevisedPrompt);
public sealed record GenerateImageRequest([Required, StringLength(1000, MinimumLength = 3)] string Prompt,
    [RegularExpression("^(square|portrait|landscape)$")] string Layout = "square");

public interface IImageGenerationProvider
{
    ImageGenerationCapability Capability { get; }
    Task<GeneratedImage> GenerateAsync(string prompt, string layout, CancellationToken ct);
}

public sealed class OpenAiCompatibleImageGenerationProvider(
    ImageGenerationSettings settings,
    IHttpClientFactory clients,
    ILogger<OpenAiCompatibleImageGenerationProvider> logger) : IImageGenerationProvider
{
    public ImageGenerationCapability Capability => settings.IsConfigured
        ? new(true, "READY", settings.Provider, settings.Model)
        : new(false, "IMAGE_PROVIDER_NOT_CONFIGURED", settings.Provider, null);

    public async Task<GeneratedImage> GenerateAsync(string prompt, string layout, CancellationToken ct)
    {
        if (!settings.IsConfigured)
            throw new InvalidOperationException("Image generation is not configured. Add an image provider in Render.");

        var size = layout switch { "portrait" => "1024x1536", "landscape" => "1536x1024", _ => "1024x1024" };
        using var request = new HttpRequestMessage(HttpMethod.Post, settings.BaseUrl.TrimEnd('/') + "/images/generations");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", settings.ApiKey);
        request.Content = new StringContent(JsonSerializer.Serialize(new
        {
            model = settings.Model,
            prompt = prompt.Trim(),
            size,
            n = 1
        }), Encoding.UTF8, "application/json");

        using var timeout = CancellationTokenSource.CreateLinkedTokenSource(ct);
        timeout.CancelAfter(TimeSpan.FromSeconds(settings.TimeoutSeconds));
        using var response = await clients.CreateClient("image-generation").SendAsync(request, HttpCompletionOption.ResponseHeadersRead, timeout.Token);
        var json = await response.Content.ReadAsStringAsync(timeout.Token);
        if (!response.IsSuccessStatusCode)
        {
            logger.LogWarning("Image provider failed with status {StatusCode}", (int)response.StatusCode);
            throw new InvalidOperationException(response.StatusCode == System.Net.HttpStatusCode.TooManyRequests
                ? "The image provider is busy. Please wait and try again."
                : "The image provider could not create the image. Please try again.");
        }

        using var document = JsonDocument.Parse(json);
        var item = document.RootElement.GetProperty("data")[0];
        var revised = item.TryGetProperty("revised_prompt", out var rp) ? rp.GetString() ?? prompt.Trim() : prompt.Trim();
        if (!item.TryGetProperty("b64_json", out var encoded) || string.IsNullOrWhiteSpace(encoded.GetString()))
            throw new InvalidOperationException("The image provider returned an unsupported response.");
        var bytes = Convert.FromBase64String(encoded.GetString()!);
        if (bytes.Length is < 100 or > 15 * 1024 * 1024)
            throw new InvalidOperationException("The generated image size was invalid.");
        return new($"data:image/png;base64,{Convert.ToBase64String(bytes)}", "image/png", settings.Provider, settings.Model, revised);
    }
}

[ApiController, Authorize(Roles = "User"), Route("api/images"), ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
public sealed class ImagesController(IImageGenerationProvider provider) : ControllerBase
{
    [HttpGet("capability")]
    public IActionResult Capability() => Ok(provider.Capability);

    [HttpPost("generate"), EnableRateLimiting("imageGeneration"), RequestSizeLimit(8_000)]
    public async Task<IActionResult> Generate(GenerateImageRequest request, CancellationToken ct)
    {
        if (!provider.Capability.Configured)
            return StatusCode(503, new { code = "IMAGE_PROVIDER_NOT_CONFIGURED", title = "Image generation needs a provider API key. Add the image settings in Render and redeploy." });
        try { return Ok(await provider.GenerateAsync(request.Prompt, request.Layout, ct)); }
        catch (OperationCanceledException) when (!ct.IsCancellationRequested)
        { return StatusCode(504, new { title = "Image generation took too long. Please try again." }); }
        catch (InvalidOperationException ex)
        { return StatusCode(502, new { title = ex.Message }); }
    }
}
