using Microsoft.EntityFrameworkCore;
using Spilton.Api.Data;
namespace Spilton.Api.Government;
public sealed record ResearchSource(Guid DocumentId,string Title,string SourceType,string Verification,string? Url);
public interface IResearchSourceProvider
{
 bool LiveSearchAvailable{get;}
 Task<IReadOnlyList<ResearchSource>> Sources(Guid user,Guid? space,Guid[] documentIds,CancellationToken ct);
}
// A future free search connector can implement this boundary; this provider never fetches a URL.
public sealed class ManualResearchSources(AppDbContext db):IResearchSourceProvider
{
 public bool LiveSearchAvailable=>false;
 public async Task<IReadOnlyList<ResearchSource>> Sources(Guid user,Guid? space,Guid[] documentIds,CancellationToken ct){
  var docs=await db.Documents.AsNoTracking().Where(d=>d.UserId==user&&d.SpaceId==space&&documentIds.Contains(d.Id)&&d.Status=="READY").Select(d=>new{d.Id,d.OriginalName}).ToListAsync(ct);
  var records=await db.GovernmentResources.AsNoTracking().Where(r=>r.UserId==user&&r.SpaceId==space&&r.DocumentId.HasValue&&documentIds.Contains(r.DocumentId.Value)).ToListAsync(ct);
  return docs.Select(d=>{var r=records.Where(r=>r.DocumentId==d.Id).OrderByDescending(r=>SourcePolicy.Rank(r.SourceType)).FirstOrDefault();return new ResearchSource(d.Id,d.OriginalName,r?.SourceType??"USER_UPLOADED",r?.Verification??"UNVERIFIED_CONTENT",r?.SourceUrl);}).OrderByDescending(s=>SourcePolicy.Rank(s.SourceType)).ToArray();
 }
}
