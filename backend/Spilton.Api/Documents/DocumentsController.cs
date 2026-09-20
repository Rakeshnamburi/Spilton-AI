using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Spilton.Api.Chat;
using Spilton.Api.Data;
using Spilton.Api.Preparation;
namespace Spilton.Api.Documents;
[ApiController,Authorize(Roles="User"),Route("api/documents"),ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
public sealed class DocumentsController(AppDbContext db,IFileStorage storage,RetrievalService retrieval,IWebHostEnvironment environment,RagSettings settings,ScopeGuard scopes):ControllerBase
{
    private Guid UserId=>Guid.Parse(User.FindFirst("sub")!.Value);
    private IQueryable<Document> Owned=>db.Documents.Where(d=>d.UserId==UserId&&d.Status!="DELETING");
    private static readonly Dictionary<string,string> Types=new(){{".pdf","application/pdf"},{".txt","text/plain"},{".docx","application/vnd.openxmlformats-officedocument.wordprocessingml.document"}};
    [HttpPost,EnableRateLimiting("documents"),RequestSizeLimit(5300000),RequestFormLimits(MultipartBodyLengthLimit=5300000)]
    public async Task<IActionResult> Upload([FromForm]IFormFile file,[FromForm]string category="OTHER",[FromForm]Guid? spaceId=null,CancellationToken ct=default)
    {
        if(!await scopes.Allowed(UserId,spaceId,true,ct))return NotFound();
        if(file.Length is 0 or >RagSettings.MaxFileBytes)return Problem(statusCode:413,title:"Upload a nonempty file of at most 5 MB.");
        var name=file.FileName;
        if(!Spilton.Api.Security.UploadPolicy.SafeName(name))return Problem(statusCode:400,title:"The filename contains unsafe characters or an executable double extension.");
        var extension=Path.GetExtension(name).ToLowerInvariant();
        if(!Types.TryGetValue(extension,out var mime))return Problem(statusCode:415,title:"Supported formats are PDF, TXT and DOCX.");
        if(file.ContentType!=mime&&file.ContentType!="application/octet-stream")return Problem(statusCode:415,title:"File type and extension do not match.");
        if(!new[]{"NOTIFICATION","SYLLABUS","PREVIOUS_YEAR_PAPER","STUDY_MATERIAL","USER_NOTES","RESUME","OTHER"}.Contains(category))return Problem(statusCode:400,title:"Invalid document category.");
        await using var gate=await ConversationLock.TryAcquire(db,UserId,ct);if(gate is null)return Problem(statusCode:409,title:"Another upload is being saved. Retry shortly.");
        if(await db.Documents.CountAsync(d=>d.UserId==UserId,ct)>=30)return Problem(statusCode:409,title:"Development limit: 30 documents per user. Delete an unused document first.");
        using var input=file.OpenReadStream();var signature=new byte[5];var read=await input.ReadAsync(signature,ct);input.Position=0;
        if((extension==".pdf"&&(read<5||System.Text.Encoding.ASCII.GetString(signature)!="%PDF-"))||(extension==".docx"&&(read<2||signature[0]!=80||signature[1]!=75)))return Problem(statusCode:415,title:"The file content does not match its format.");
        var doc=new Document{UserId=UserId,SpaceId=spaceId,OriginalName=name,StoredName=Guid.NewGuid().ToString("N")+extension,ContentType=mime,Size=file.Length,Category=category};
        try{await storage.SaveAsync(doc.StoredName,input,ct);db.Documents.Add(doc);await db.SaveChangesAsync(ct);}catch{try{await storage.DeleteAsync(doc.StoredName,CancellationToken.None);}catch{}throw;}
        return Accepted($"/api/documents/{doc.Id}",DocumentDto.From(doc));
    }
    [HttpGet]public async Task<IActionResult> List([FromQuery]Guid? spaceId,CancellationToken ct){if(!await scopes.Allowed(UserId,spaceId,false,ct))return NotFound();return Ok(new{items=(await Owned.AsNoTracking().Where(d=>d.SpaceId==spaceId).OrderByDescending(d=>d.CreatedAt).ToListAsync(ct)).Select(DocumentDto.From)});}
    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> Assign(Guid id,DocumentAssignment request,CancellationToken ct){if(!await scopes.Allowed(UserId,request.SpaceId,true,ct))return NotFound();if(!await Owned.AnyAsync(d=>d.Id==id,ct))return NotFound();await using var gate=await ConversationLock.TryAcquire(db,id,ct);if(gate is null)return Conflict(new{title="Wait until document processing or generation finishes."});var doc=await Owned.SingleOrDefaultAsync(d=>d.Id==id,ct);if(doc is null)return NotFound();foreach(var resource in await db.GovernmentResources.Where(r=>r.DocumentId==id&&r.UserId==UserId).ToListAsync(ct))resource.SpaceId=request.SpaceId;doc.SpaceId=request.SpaceId;doc.UpdatedAt=DateTimeOffset.UtcNow;await db.SaveChangesAsync(ct);return Ok(DocumentDto.From(doc));}
    [HttpGet("{id:guid}")]public async Task<IActionResult> Get(Guid id,CancellationToken ct){var doc=await Owned.AsNoTracking().SingleOrDefaultAsync(d=>d.Id==id,ct);return doc is null?NotFound():Ok(DocumentDto.From(doc));}
    [HttpGet("{id:guid}/chunks/{chunkId:guid}")]public async Task<IActionResult> Source(Guid id,Guid chunkId,CancellationToken ct){var chunk=await db.DocumentChunks.AsNoTracking().Include(c=>c.Document).SingleOrDefaultAsync(c=>c.Id==chunkId&&c.DocumentId==id&&c.Document.UserId==UserId&&c.Document.Status=="READY",ct);return chunk is null?NotFound():Ok(new{chunk.Content,chunk.PageNumber,chunk.Section,name=chunk.Document.OriginalName});}
    [HttpDelete("{id:guid}")]public async Task<IActionResult> Delete(Guid id,CancellationToken ct){if(!await Owned.AnyAsync(d=>d.Id==id,ct))return NotFound();await using var gate=await ConversationLock.TryAcquire(db,id,ct);if(gate is null)return Problem(statusCode:409,title:"This document is processing. Retry when processing finishes.");var doc=await Owned.SingleOrDefaultAsync(d=>d.Id==id,ct);if(doc is null)return NotFound();
        // Tombstone commits first: retrieval cannot see a half-deleted document. Worker retries disk failures.
        doc.Status="DELETING";await db.SaveChangesAsync(ct);await db.DocumentChunks.Where(c=>c.DocumentId==id).ExecuteDeleteAsync(ct);
        try{await storage.DeleteAsync(doc.StoredName,ct);db.Documents.Remove(doc);await db.SaveChangesAsync(ct);}catch(IOException){return Accepted();}return NoContent();}
    [HttpPost("diagnostics"),EnableRateLimiting("chat")]
    public async Task<IActionResult> Diagnostics(RetrievalRequest request,CancellationToken ct){if(!environment.IsDevelopment()||!settings.Diagnostics)return NotFound();try{return Ok(await retrieval.Search(UserId,request.DocumentIds,request.Question,ct,request.SpaceId));}catch(DocumentException ex){return Problem(statusCode:400,title:ex.Message);}}
}
public sealed record RetrievalRequest([Required,StringLength(2000,MinimumLength=1)]string Question,[Required,MaxLength(5),MinLength(1)]Guid[] DocumentIds,Guid? SpaceId=null);
public sealed record DocumentAssignment(Guid? SpaceId);
