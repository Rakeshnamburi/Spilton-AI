namespace Spilton.Api.Security;

public static class UploadPolicy
{
    private static readonly HashSet<string> DangerousSegments = new(StringComparer.OrdinalIgnoreCase)
        { "exe", "com", "bat", "cmd", "ps1", "vbs", "js", "scr", "msi", "dll", "html", "htm", "svg" };
    public static bool SafeName(string name) => name.Length is > 0 and <= 200 &&
        !name.Any(char.IsControl) && name.IndexOfAny(['/', '\\', ':']) < 0 &&
        !name.EndsWith(' ') && !name.EndsWith('.') &&
        !name.Split('.').Skip(1).SkipLast(1).Any(DangerousSegments.Contains);
}
