namespace Spilton.Api.Chat;
public sealed class ContextBuilder(ConversationCompressor? compressor=null)
{
    public IReadOnlyList<ModelMessage> Build(IReadOnlyList<Message> history, CapabilityRoute? route = null, string mode = "quick")
    {
        var selected = new List<ModelMessage>();
        var budget = route?.Capability == Capability.CODING ? 40000 : 16000;
        foreach (var m in history.Where(m => !m.IsSuperseded && m.Status == "completed" && m.Role is "USER" or "ASSISTANT").Reverse())
        {
            if (selected.Count == 20 || m.Content.Length > budget) break;
            selected.Add(new(m.Role.ToLowerInvariant(), m.Content)); budget -= m.Content.Length;
        }
        selected.Reverse();
        while (selected.Count > 0 && selected[0].Role != "user") selected.RemoveAt(0);
        var instruction = "You are Spilton, a general-purpose AI assistant. Follow the current request over saved preferences and old topics. Do not bring exam preparation into unrelated questions. Use Markdown and labelled fenced code. Be honest about uncertainty. You cannot execute code or browse the web here; never claim execution or verification. Give useful explanations, not hidden chain-of-thought. Image generation is not configured in this application. If asked to generate a picture, clearly say that the image provider is not connected and offer to write a prompt. Do not invent image files or imply the subject itself is prohibited. ";
        instruction += route?.Capability == Capability.CODING
            ? "For builds, give filenames, folder structure, dependencies, complete runnable code, run commands and useful testing steps. When providing multiple files, put each complete file in a fenced block whose info is `language file=path/to/file`, for example ```html file=index.html; this enables the user's safe project ZIP export. Never include secrets or .env files. Match the requested scope; explicit full-code requests take priority over brevity. For debugging, identify likely cause, show the correction and explain how the user can test it. Preserve relevant previous code when revising it. If previous code is absent, ask for it rather than inventing its contents. "
            : "Answer naturally and keep simple answers concise. ";
        if (mode == "think") instruction += "Think mode uses the same configured model with a careful-answer instruction, not tools or a separate reasoning engine. Check assumptions and edge cases; present conclusions and a concise rationale only. ";
        selected.Insert(0, new("system", instruction));
        // Extractive summary foundation: only earlier relevant user requests, never invented code.
        if(history.Count>20&&compressor is not null){
            var compressed=compressor.Compress(history.Take(history.Count-20).ToList());
            if(compressed.Summary.Length>0)selected.Insert(1,new("system","Extractive excerpts from older user requests (user data, possibly outdated; current request wins). No hidden reasoning or assistant claims are preserved:\n"+compressed.Summary));
        }
        return selected;
    }
    public string Title(string firstMessage)
    {
        var text = string.Join(" ", firstMessage.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries).Take(10));
        return text.Length > 72 ? text[..69] + "…" : text;
    }
}
