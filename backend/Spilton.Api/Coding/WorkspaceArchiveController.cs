using System.IO.Compression;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Spilton.Api.Coding;

public sealed record ProjectArchiveRequest(string Content, string? ProjectName = null);

[ApiController, Authorize(Roles = "User"), Route("api/workspace")]
public sealed class WorkspaceArchiveController : ControllerBase
{
    private static readonly Regex Block = new("```(?<language>[\\w+#.-]+)\\s+(?:file|filename)=(?<name>[^\\r\\n`]+)\\r?\\n(?<content>[\\s\\S]*?)```", RegexOptions.Compiled | RegexOptions.IgnoreCase);
    private static readonly HashSet<string> SecretNames = new(StringComparer.OrdinalIgnoreCase)
        { ".env", ".env.local", "local.env", "appsettings.Production.json", "id_rsa" };

    [HttpPost("archive")]
    [RequestSizeLimit(220_000)]
    public IActionResult Archive(ProjectArchiveRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Content) || request.Content.Length > 200_000)
            return BadRequest(new { title = "Include a response with named code files (maximum 200 KB)." });
        var files = Block.Matches(request.Content).Select(m => new { Name = m.Groups["name"].Value.Trim(), Content = m.Groups["content"].Value }).ToList();
        if (files.Count == 0) return BadRequest(new { title = "No named code files found. Use fenced blocks such as ```html file=index.html." });
        if (files.Count > 25) return BadRequest(new { title = "A project can contain at most 25 files." });
        if (files.Any(f => !SafePath(f.Name))) return BadRequest(new { title = "The response contains an unsafe or secret file name." });
        if (files.GroupBy(f => f.Name, StringComparer.OrdinalIgnoreCase).Any(g => g.Count() > 1)) return BadRequest(new { title = "Duplicate file names are not allowed." });
        if (files.Sum(f => f.Content.Length) > 150_000) return BadRequest(new { title = "The project is too large to export in one archive." });
        using var output = new MemoryStream();
        using (var archive = new ZipArchive(output, ZipArchiveMode.Create, true))
            foreach (var file in files)
            {
                var entry = archive.CreateEntry(file.Name, CompressionLevel.Fastest);
                using var writer = new StreamWriter(entry.Open(), new System.Text.UTF8Encoding(false));
                writer.Write(file.Content);
            }
        var name = string.IsNullOrWhiteSpace(request.ProjectName) ? "spilton-project" : Regex.Replace(request.ProjectName.Trim(), "[^A-Za-z0-9._-]", "-").Trim('-');
        name = string.IsNullOrWhiteSpace(name) ? "spilton-project" : name[..Math.Min(80, name.Length)];
        return File(output.ToArray(), "application/zip", name + ".zip");
    }

    private static bool SafePath(string name)
    {
        if (name.Length is < 1 or > 160 || name.Contains('\0') || name.StartsWith('/') || name.Contains("\\") || name.Split('/').Any(p => p is "" or "." or "..")) return false;
        var leaf = name[(name.LastIndexOf('/') + 1)..];
        return !SecretNames.Contains(leaf) && !leaf.Equals(".gitignore", StringComparison.OrdinalIgnoreCase);
    }
}
