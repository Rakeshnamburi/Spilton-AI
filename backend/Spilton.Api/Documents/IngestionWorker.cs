using Microsoft.EntityFrameworkCore;
using Pgvector;
using Spilton.Api.Chat;
using Spilton.Api.Data;
using Spilton.Api.Infrastructure;
namespace Spilton.Api.Documents;
public sealed class IngestionWorker(IServiceScopeFactory scopes,ILogger<IngestionWorker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var backoff = new WorkerBackoff(TimeSpan.FromSeconds(1), TimeSpan.FromMinutes(1));
        while(!stoppingToken.IsCancellationRequested){
            var delay = TimeSpan.FromSeconds(1);
            try{using var scope=scopes.CreateScope();var db=scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var id=await db.Documents.Where(d=>d.Status=="UPLOADED"||d.Status=="DELETING"||(d.Status=="PROCESSING"&&d.UpdatedAt<DateTimeOffset.UtcNow.AddMinutes(-5))).OrderBy(d=>d.CreatedAt).Select(d=>(Guid?)d.Id).FirstOrDefaultAsync(stoppingToken);
                if(id.HasValue){await Process(scope.ServiceProvider,id.Value,stoppingToken);backoff.Reset();continue;}
                backoff.Reset();
            }catch(OperationCanceledException)when(stoppingToken.IsCancellationRequested){break;}
            catch(Exception ex){delay=backoff.NextFailureDelay();logger.LogWarning("Document worker temporarily unavailable; retrying with backoff. ErrorCategory={ErrorCategory} ErrorType={ErrorType} ConsecutiveFailures={ConsecutiveFailures} RetryDelaySeconds={RetryDelaySeconds}",ex is DbUpdateException?"database":"worker",ex.GetType().Name,backoff.ConsecutiveFailures,delay.TotalSeconds);}
            try{await Task.Delay(delay,stoppingToken);}catch(OperationCanceledException)when(stoppingToken.IsCancellationRequested){break;}
        }
    }
    private static async Task Process(IServiceProvider services,Guid id,CancellationToken ct)
    {
        var db=services.GetRequiredService<AppDbContext>();await using var gate=await ConversationLock.TryAcquire(db,id,ct);if(gate is null){await Task.Delay(500,ct);return;}
        var doc=await db.Documents.SingleOrDefaultAsync(d=>d.Id==id,ct);if(doc is null)return;
        var storage=services.GetRequiredService<IFileStorage>();
        if(doc.Status=="DELETING"){storage.Delete(doc.StoredName);db.Documents.Remove(doc);await db.SaveChangesAsync(ct);return;}
        if(doc.Status=="PROCESSING"){doc.Status="FAILED";doc.Error="Processing was interrupted. Delete and upload the document again.";await db.SaveChangesAsync(ct);return;}
        doc.Status="PROCESSING";doc.UpdatedAt=DateTimeOffset.UtcNow;await db.SaveChangesAsync(ct);
        try{
            using var timeout=CancellationTokenSource.CreateLinkedTokenSource(ct);timeout.CancelAfter(TimeSpan.FromMinutes(3));
            using var input=storage.Open(doc.StoredName);
            var extraction=services.GetRequiredService<DocumentExtractor>().Extract(input,Path.GetExtension(doc.StoredName),timeout.Token);
            var chunks=services.GetRequiredService<DocumentChunker>().Split(extraction);
            var embeddings=services.GetRequiredService<IEmbeddingProvider>();
            var vectors=await embeddings.EmbedAsync(chunks.Select(c=>c.Content).ToArray(),timeout.Token);
            if(vectors.Length!=chunks.Count||vectors.Any(v=>v.Length!=384||v.Any(x=>!float.IsFinite(x))))throw new DocumentException("Invalid embedding output.");
            db.DocumentChunks.AddRange(chunks.Select((c,i)=>new DocumentChunk{DocumentId=id,ChunkIndex=i,Content=c.Content,PageNumber=c.Page,Section=c.Section,TokenCount=c.Tokens,Embedding=new Vector(vectors[i])}));
            doc.PageCount=extraction.PageCount;doc.ChunkCount=chunks.Count;doc.EmbeddingModel=embeddings.Model;doc.Status="READY";doc.UpdatedAt=DateTimeOffset.UtcNow;
            await db.SaveChangesAsync(timeout.Token);
        }catch(Exception ex){
            db.ChangeTracker.Clear();doc=await db.Documents.SingleAsync(d=>d.Id==id,CancellationToken.None);doc.Status="FAILED";
            doc.Error=ex is DocumentException?ex.Message:ex is OperationCanceledException?"Processing timed out or was interrupted. Try a smaller document.":"Unable to process this document. Check the file format and local embedding model installation.";
            doc.UpdatedAt=DateTimeOffset.UtcNow;using var save=new CancellationTokenSource(TimeSpan.FromSeconds(10));await db.SaveChangesAsync(save.Token);
        }
    }
}
