using Pgvector;
using Spilton.Api.Data;
namespace Spilton.Api.Documents;

// One uploaded file produces one document today; a second Files table would duplicate metadata.
public sealed class Document
{
    public Guid? SpaceId {get;set;}public Spilton.Api.Preparation.Space? Space {get;set;}
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public string OriginalName { get; set; } = "";
    public string StoredName { get; set; } = "";
    public string ContentType { get; set; } = "";
    public long Size { get; set; }
    public string Category { get; set; } = "OTHER";
    public string Status { get; set; } = "UPLOADED";
    public string? Error { get; set; }
    public int? PageCount { get; set; }
    public int ChunkCount { get; set; }
    public string? EmbeddingModel { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
    public ICollection<DocumentChunk> Chunks { get; set; } = new List<DocumentChunk>();
}
public sealed class DocumentChunk
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid DocumentId { get; set; }
    public Document Document { get; set; } = null!;
    public int ChunkIndex { get; set; }
    public string Content { get; set; } = "";
    public int? PageNumber { get; set; }
    public string? Section { get; set; }
    public int TokenCount { get; set; }
    public Vector Embedding { get; set; } = null!;
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}
public sealed record Citation(int Number, Guid DocumentId, Guid ChunkId, string Name, int? Page, string? Section, string Excerpt, string? Url=null, DateTimeOffset? RetrievedAt=null, string? SourceType=null);
public sealed record DocumentDto(Guid Id, string Name, string ContentType, long Size, string Category, string Status, string? Error, int? PageCount, int ChunkCount, DateTimeOffset CreatedAt,Guid? SpaceId)
{
    public static DocumentDto From(Document d) => new(d.Id,d.OriginalName,d.ContentType,d.Size,d.Category,d.Status,d.Error,d.PageCount,d.ChunkCount,d.CreatedAt,d.SpaceId);
}
public sealed class RagSettings
{
    public string StoragePath { get; set; } = ".local/documents";
    public string ModelPath { get; set; } = ".local/models/minilm";
    public int ChunkTokens { get; set; } = 200;
    public int OverlapTokens { get; set; } = 30;
    public int TopK { get; set; } = 6;
    public double MinimumSimilarity { get; set; } = 0.25;
    public bool Diagnostics { get; set; }
    public const long MaxFileBytes = 5 * 1024 * 1024;
    public const int MaxChunks = 500;
}
public sealed class DocumentException(string message) : Exception(message);
