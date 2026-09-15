using System.Text;

namespace Spilton.Api.Coding;

public sealed record WorkspaceFile(string Path,string Content,long SizeBytes);
public sealed record FilePatch(string Path,string? ExpectedContent,string NewContent);
public sealed record PatchPreview(string Path,bool CreatesFile,int AddedLines,int RemovedLines,string Status);
public sealed record ExecutionCapability(bool Available,string Status,string Boundary);

public sealed class CodingWorkspacePolicy
{
    public const int MaxFiles=40;
    public const int MaxFileBytes=256_000;
    public const int MaxPatchBytes=512_000;
    private static readonly HashSet<string> SecretNames=new(StringComparer.OrdinalIgnoreCase)
    { ".env",".env.local","id_rsa","id_ed25519","credentials","credentials.json","secrets.json" };
    public string Resolve(string root,string relative)
    {
        if(string.IsNullOrWhiteSpace(relative)||Path.IsPathRooted(relative)||relative.IndexOfAny(Path.GetInvalidPathChars())>=0)
            throw new ArgumentException("Choose a relative workspace path.");
        var normalized=relative.Replace('/',Path.DirectorySeparatorChar);
        if(normalized.Split(Path.DirectorySeparatorChar).Any(x=>x is ".." or "."||SecretNames.Contains(x)))
            throw new UnauthorizedAccessException("That path is outside the allowed coding workspace or contains a protected file.");
        var full=Path.GetFullPath(Path.Combine(root,normalized));
        var prefix=Path.GetFullPath(root).TrimEnd(Path.DirectorySeparatorChar)+Path.DirectorySeparatorChar;
        if(!full.StartsWith(prefix,StringComparison.OrdinalIgnoreCase))throw new UnauthorizedAccessException("That path is outside the allowed coding workspace.");
        return full;
    }
}

public interface ICodingWorkspace
{
    Task<IReadOnlyList<string>> Tree(CancellationToken ct);
    Task<WorkspaceFile> Read(string path,CancellationToken ct);
    Task<IReadOnlyList<PatchPreview>> Preview(IReadOnlyList<FilePatch> patches,CancellationToken ct);
}

// Filesystem workspaces must be provisioned per user in an isolated root. The
// application repository is deliberately never exposed as a user workspace.
public sealed class UnavailableCodingWorkspace : ICodingWorkspace
{
    private static InvalidOperationException Unavailable()=>new("A per-user isolated coding workspace is not configured.");
    public Task<IReadOnlyList<string>> Tree(CancellationToken ct)=>Task.FromException<IReadOnlyList<string>>(Unavailable());
    public Task<WorkspaceFile> Read(string path,CancellationToken ct)=>Task.FromException<WorkspaceFile>(Unavailable());
    public Task<IReadOnlyList<PatchPreview>> Preview(IReadOnlyList<FilePatch> patches,CancellationToken ct)=>Task.FromException<IReadOnlyList<PatchPreview>>(Unavailable());
}

public interface ICodeExecutionService
{
    ExecutionCapability Capability { get; }
    Task<string> ExecuteAsync(string workspaceId,IReadOnlyList<string> command,CancellationToken ct);
}
public sealed class UnavailableCodeExecutionService : ICodeExecutionService
{
    public ExecutionCapability Capability=>new(false,"ISOLATED_EXECUTION_NOT_CONFIGURED","No unrestricted host shell is exposed.");
    public Task<string> ExecuteAsync(string workspaceId,IReadOnlyList<string> command,CancellationToken ct)=>Task.FromException<string>(new InvalidOperationException("Controlled code execution is unavailable."));
}

