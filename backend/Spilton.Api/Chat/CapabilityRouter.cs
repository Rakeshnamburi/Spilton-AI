using System.Text.RegularExpressions;
namespace Spilton.Api.Chat;

public enum Capability { GENERAL, CODING, TUTOR, REASONING, EXAM, DOCUMENT_RAG, RESEARCH, AGENT }
public sealed record CapabilityRoute(Capability Capability, bool UseExamContext, string Reason);

// No classification model call: uncertain requests use the general model to answer directly.
public sealed class CapabilityRouter
{
    private static bool Has(string text, string pattern) => Regex.IsMatch(text, pattern, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
    public CapabilityRoute Route(string text, IReadOnlyList<Message> history, bool documents, bool examProfile, string mode)
    {
        if (documents || Has(text,@"\b(from|based on|according to)\b.*\b(pdf|uploaded|document|handbook)\b")) return new(Capability.DOCUMENT_RAG, false, "Document evidence requested");
        if(mode=="agent")return new(Capability.AGENT,false,"Explicit bounded agent mode");
        if (mode == "research" || Has(text, @"\b(latest|today'?s?|current|recent|newest)\b.*\b(changes|news|version|release|notification|affairs|vacanc|updates|price)|\b(deadline|last date)\b"))
            return new(Capability.RESEARCH, false, "Current evidence required");
        if (Has(text, @"```|\b(html|css|javascript|typescript|react|next\.js|python|java|c#|csharp|asp\.net|\.net|sql|code|coding|program|compiler|stack trace|exception|dependency injection)\b|\b(build|create|make)\b.*\b(page|app|website|api|component)\b"))
            return new(Capability.CODING, false, "Software task");
        if (Has(text, @"\b(ssc|cgl|chsl|rrb|ntpc|appsc|upsc|government exam|banking exam)\b")) return new(Capability.EXAM, true, "Explicit exam request");
        if (Has(text, @"\b(make it|convert it|change the design|previous code|continue|responsive)\b") && history.Where(m => m.Role == "USER" && m.Content != text && !m.IsSuperseded).TakeLast(4).Reverse().Any(m => Has(m.Content, @"\b(html|css|react|python|code|program|login page|component)\b")))
            return new(Capability.CODING, false, "Recent coding follow-up");
        if (examProfile && Has(text, @"\b(teach|practice|explain|revise)\b.*\b(percentage|percentages|quant|arithmetic|profit|reasoning|syllabus)\b")) return new(Capability.EXAM, true, "Relevant preparation topic");
        if (mode == "think" || Has(text, @"\b(prove|solve|reason through|compare tradeoffs|plan)\b")) return new(Capability.REASONING, false, "Reasoning task");
        if (Has(text, @"\b(teach|explain|learn)\b")) return new(Capability.TUTOR, false, "Learning request");
        return new(Capability.GENERAL, false, "General fallback");
    }
}

// Capability-to-model policy boundary; today all capabilities use the configured default.
public sealed record ModelSelection(IModelProvider Provider,string Reason,string Health,bool ExplicitSelection);
public sealed class CapabilityModelRouter(IModelProviderResolver providers,IProviderHealth? health=null)
{
    public ModelSelection Select(CapabilityRoute route,string? requestedModel)
    {
        var explicitSelection=!string.IsNullOrWhiteSpace(requestedModel)&&requestedModel!="auto";
        var provider=providers.Resolve(requestedModel);
        var capabilities=provider.Info.Capabilities;
        var supported=route.Capability switch
        {
            Capability.CODING=>capabilities.Coding,
            Capability.REASONING=>capabilities.Reasoning,
            Capability.RESEARCH=>capabilities.ResearchSynthesis,
            _=>capabilities.General
        };
        if(!supported)throw new ProviderException("model_capability_unavailable","The selected configured model does not support this capability.");
        return new(provider,explicitSelection?"User selected a configured model":$"Configured default selected for {route.Capability}",health?.Status(provider.Info.Id)??"UNKNOWN",explicitSelection);
    }
    public IModelProvider Resolve(CapabilityRoute route, string? requestedModel)
    {
        // Provider availability is authoritative; capability policy never invents a model.
        return Select(route,requestedModel).Provider;
    }
}

public sealed record ContextCompression(string Summary, int OriginalMessages, int IncludedMessages);
public sealed class ConversationCompressor
{
    public ContextCompression Compress(IReadOnlyList<Message> messages, int maxChars=2400)
    {
        var relevant=messages.Where(m=>m.Role=="USER"&&m.Status=="completed").Take(20).Select(m=>m.Content.Trim()).Where(x=>x.Length>0).ToList();
        var text=string.Join(" ",relevant.TakeLast(6)); if(text.Length>maxChars)text=text[..maxChars];
        return new(text,relevant.Count,Math.Min(6,relevant.Count));
    }
}
