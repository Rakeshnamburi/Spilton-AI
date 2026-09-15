using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text.Json;
namespace Spilton.Api.Chat;

public sealed record ModelCapabilities(
    bool General=true,
    bool Coding=true,
    bool Reasoning=true,
    bool ResearchSynthesis=true,
    bool Vision=false,
    int MaxContextCharacters=16000,
    string LatencyClass="STANDARD",
    string CostClass="CONFIGURED_PROVIDER");
public sealed record ModelInfo(string Id, string Label, string Provider, string Model, bool IsDevelopment)
{
    public ModelCapabilities Capabilities { get; init; } = new();
    public string Availability { get; init; } = "CONFIGURED";
}
public sealed record ModelMessage(string Role, string Content);
public interface IModelProvider
{
    ModelInfo Info { get; }
    IAsyncEnumerable<string> StreamAsync(IReadOnlyList<ModelMessage> messages, CancellationToken ct);
}
public sealed class ProviderException(string code, string safeMessage) : Exception(safeMessage)
{ public string Code { get; } = code; }
public sealed class ModelSettings
{
    public bool DevelopmentEnabled { get; set; }
    public string Default { get; set; } = "development";
    public int TimeoutSeconds { get; set; } = 90;
    public string BaseUrl { get; set; } = "";
    public string ApiKey { get; set; } = "";
    public string Model { get; set; } = "";
    public string ProviderLabel { get; set; } = "Compatible provider";
}
public interface IModelProviderResolver
{
    IReadOnlyList<ModelInfo> Available { get; }
    string DefaultId { get; }
    IModelProvider Resolve(string? id);
}
public interface IProviderHealth
{
    string Status(string providerId);
    void Success(string providerId);
    void Failure(string providerId, string code);
}
// Process-local health is advisory. It never advertises an unconfigured model and
// never records prompts, credentials or response content.
public sealed class ProviderHealth(TimeProvider time) : IProviderHealth
{
    private sealed record State(int ConsecutiveFailures,DateTimeOffset LastFailure);
    private readonly System.Collections.Concurrent.ConcurrentDictionary<string,State> states=new(StringComparer.OrdinalIgnoreCase);
    public string Status(string id)=>states.TryGetValue(id,out var s)&&s.ConsecutiveFailures>=3&&time.GetUtcNow()-s.LastFailure<TimeSpan.FromMinutes(2)?"DEGRADED":"HEALTHY";
    public void Success(string id)=>states.TryRemove(id,out _);
    public void Failure(string id,string code){if(code is not ("provider_failure" or "connection_interrupted" or "rate_limit"))return;states.AddOrUpdate(id,_=>new(1,time.GetUtcNow()),(_,s)=>new(s.ConsecutiveFailures+1,time.GetUtcNow()));}
}
public sealed class ModelProviderFactory(ModelSettings settings, IWebHostEnvironment environment, IHttpClientFactory clients) : IModelProviderResolver
{
    public IReadOnlyList<ModelInfo> Available => Providers().Select(p => p.Info).ToList();
    public string DefaultId => settings.Default;
    private IEnumerable<IModelProvider> Providers()
    {
        if (environment.IsDevelopment() && settings.DevelopmentEnabled) yield return new DevelopmentProvider();
        if (Configured) yield return new CompatibleProvider(settings, clients.CreateClient("models"));
    }
    private bool Configured => !string.IsNullOrWhiteSpace(settings.ApiKey) && !string.IsNullOrWhiteSpace(settings.Model)
        && settings.Model.Length <= 120 && settings.ProviderLabel.Length <= 80
        && Uri.TryCreate(settings.BaseUrl, UriKind.Absolute, out var uri) && uri.Scheme == "https" && string.IsNullOrEmpty(uri.UserInfo) && string.IsNullOrEmpty(uri.Query);
    public IModelProvider Resolve(string? id)
    {
        var selected = string.IsNullOrEmpty(id) || id == "auto" ? settings.Default : id;
        return Providers().FirstOrDefault(p => p.Info.Id == selected)
            ?? throw new ProviderException("provider_unavailable", "This model is not configured. Choose an available model or ask the server administrator to configure one.");
    }
}
public sealed class DevelopmentProvider : IModelProvider
{
    public ModelInfo Info => new("development", "Development demo · not real AI", "Development", "deterministic-demo-v1", true)
    { Capabilities=new(Reasoning:false,ResearchSynthesis:false,MaxContextCharacters:8000,LatencyClass:"FAST",CostClass:"FREE_LOCAL_DEMO") };
    public async IAsyncEnumerable<string> StreamAsync(IReadOnlyList<ModelMessage> messages, [EnumeratorCancellation] CancellationToken ct)
    {
        var prompt = messages.Last(m => m.Role == "user").Content;
        var previous = messages.Count(m => m.Role == "user") - 1;
        var text = "**Development provider — not a real AI answer.**\n\n" +
            "This is a deterministic demonstration of Spilton’s streaming chat. Your message was received and will remain in this conversation.\n\n" +
            $"### Context check\nI received **{previous} earlier user message(s)** in the bounded context.\n\n" +
            "### Try the interface\n- Continue this conversation after a refresh.\n- Copy this answer or regenerate it.\n- Configure a real model on the server when you are ready.\n\n" +
            "| Feature | Status |\n| --- | --- |\n| Streaming | Working demo |\n| Real model reasoning | Not connected |\n\n" +
            "```python\nprint(\"Hello from Spilton\")\n```\n\n" +
            "Your latest message (quoted as data):\n\n" + string.Join("\n", prompt.Split('\n').Select(line => "> " + line));
        for (var i = 0; i < text.Length; i += 28)
        { await Task.Delay(45, ct); yield return text.Substring(i, Math.Min(28, text.Length - i)); }
    }
}
// Optional transport. It is never advertised unless server configuration is complete.
// Protocol tests are distinct from real-provider access tests.
public sealed class CompatibleProvider(ModelSettings settings, HttpClient client) : IModelProvider
{
    public ModelInfo Info => new("compatible", settings.ProviderLabel + " · " + settings.Model, settings.ProviderLabel, settings.Model, false)
    { Capabilities=new(MaxContextCharacters:40000,LatencyClass:"STANDARD",CostClass:"PROVIDER_FREE_QUOTA_OR_ACCOUNT") };
    public async IAsyncEnumerable<string> StreamAsync(IReadOnlyList<ModelMessage> messages, [EnumeratorCancellation] CancellationToken ct)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, settings.BaseUrl.TrimEnd('/') + "/chat/completions");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", settings.ApiKey);
        var coding = messages.Any(m => m.Role == "system" && m.Content.Contains("For builds, give filenames"));
        request.Content = JsonContent.Create(new { model = settings.Model, messages = messages.Select(m => new { role = m.Role, content = m.Content }), stream = true, max_tokens = coding ? 6000 : 2048 });
        using var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, ct);
        if (!response.IsSuccessStatusCode)
            throw new ProviderException(response.StatusCode == System.Net.HttpStatusCode.TooManyRequests ? "rate_limit" : "provider_failure",
                response.StatusCode == System.Net.HttpStatusCode.TooManyRequests ? "The model is rate limited. Please try again later." : "The model provider rejected the request. Check its server configuration or try again later.");
        using var reader = new StreamReader(await response.Content.ReadAsStreamAsync(ct));
        var finished = false;
        while (await reader.ReadLineAsync(ct) is { } line)
        {
            if (line.Length > 131072) throw new ProviderException("provider_failure", "The provider sent an invalid response.");
            if (!line.StartsWith("data:")) continue;
            var data = line[5..].Trim();
            if (data == "[DONE]") { finished = true; break; }
            if (data.Length == 0) continue;
            using var json = JsonDocument.Parse(data);
            if (json.RootElement.TryGetProperty("error", out _)) throw new ProviderException("provider_failure", "The model provider could not finish the response.");
            if (!json.RootElement.TryGetProperty("choices", out var choices)) continue;
            foreach (var choice in choices.EnumerateArray())
            {
                if (choice.TryGetProperty("finish_reason", out var reason) && reason.ValueKind == JsonValueKind.String) {
                    if(reason.GetString()=="length") throw new ProviderException("response_limit","The model reached its output limit. The partial response is saved; ask it to continue from the last file or code line.");
                    finished = true;
                }
                if (choice.TryGetProperty("delta", out var delta) && delta.TryGetProperty("content", out var content) && content.ValueKind == JsonValueKind.String)
                    yield return content.GetString()!;
            }
        }
        if (!finished) throw new ProviderException("connection_interrupted", "The provider connection ended before the answer completed. You can regenerate the answer.");
    }
}
