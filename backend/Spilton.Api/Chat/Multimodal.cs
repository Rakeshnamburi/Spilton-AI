namespace Spilton.Api.Chat;
public enum MediaKind { IMAGE, DOCUMENT, AUDIO }
public sealed record MediaPart(string Kind,string MimeType,string? StorageReference,long SizeBytes=0,string? FileName=null);
public sealed record MultimodalCapability(bool Configured,string Status,IReadOnlyList<string> AcceptedTypes,long MaxBytes);
public static class MultimodalPolicy
{
    public const long MaxImageBytes=8*1024*1024;
    private static readonly HashSet<string> Images=new(StringComparer.OrdinalIgnoreCase){"image/png","image/jpeg","image/webp","image/gif"};
    public static bool IsSafe(MediaPart part)=>part.Kind=="image"&&part.SizeBytes is >0 and <=MaxImageBytes&&Images.Contains(part.MimeType)&&!string.IsNullOrWhiteSpace(part.StorageReference)&&!part.StorageReference.Contains("..",StringComparison.Ordinal);
}
public interface IMultimodalProvider
{
    bool IsConfigured { get; }
    MultimodalCapability Capability { get; }
    Task<string> DescribeAsync(IReadOnlyList<MediaPart> media,string prompt,CancellationToken ct);
}
// Deliberately unregistered until a configured provider supports vision safely.
public sealed class UnavailableMultimodalProvider : IMultimodalProvider
{
    public bool IsConfigured=>false;
    public MultimodalCapability Capability=>new(false,"VISION_PROVIDER_NOT_CONFIGURED",["image/png","image/jpeg","image/webp","image/gif"],MultimodalPolicy.MaxImageBytes);
    public Task<string> DescribeAsync(IReadOnlyList<MediaPart> media,string prompt,CancellationToken ct)=>Task.FromException<string>(new ProviderException("multimodal_unavailable","Image understanding is not configured."));
}
