using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Spilton.Api.Data;
namespace Spilton.Api.Preparation;
public sealed record SpaceRequest([Required,StringLength(100)]string Name,[StringLength(500)]string Description="",[StringLength(30)]string Type="GENERAL",bool IsArchived=false);
public sealed class ScopeGuard(AppDbContext db)
{
    public async Task<bool> Allowed(Guid user,Guid? space,bool write,CancellationToken ct)=>space is null||await db.Spaces.AnyAsync(s=>s.Id==space&&s.UserId==user&&(!write||!s.IsArchived),ct);
}
[ApiController,Authorize(Roles="User"),Route("api/spaces"),ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
public sealed class SpacesController(AppDbContext db):ControllerBase
{
    private Guid UserId=>Guid.Parse(User.FindFirst("sub")!.Value);
    private IQueryable<Space> Owned=>db.Spaces.Where(s=>s.UserId==UserId);
    [HttpGet]public async Task<IActionResult> List(CancellationToken ct)=>Ok(new{items=await Owned.AsNoTracking().OrderBy(s=>s.IsArchived).ThenBy(s=>s.Name).Select(s=>new{s.Id,s.Name,s.Description,s.Type,s.IsArchived,s.CreatedAt,s.UpdatedAt}).ToListAsync(ct)});
    [HttpGet("{id:guid}")]public async Task<IActionResult> Get(Guid id,CancellationToken ct){var s=await Owned.AsNoTracking().SingleOrDefaultAsync(s=>s.Id==id,ct);return s is null?NotFound():Ok(new{s.Id,s.Name,s.Description,s.Type,s.IsArchived,s.CreatedAt,s.UpdatedAt});}
    [HttpPost,EnableRateLimiting("chat")]
    public async Task<IActionResult> Create(SpaceRequest request,CancellationToken ct){if(!new[]{"GENERAL","EXAM","LEARNING","CAREER"}.Contains(request.Type))return BadRequest(new{title="Invalid Space type."});if(await Owned.CountAsync(ct)>=30)return Conflict(new{title="Development limit: 30 Spaces."});var s=new Space{UserId=UserId,Name=request.Name.Trim(),Description=request.Description.Trim(),Type=request.Type};db.Spaces.Add(s);await db.SaveChangesAsync(ct);return Created($"/api/spaces/{s.Id}",new{s.Id,s.Name,s.Description,s.Type,s.IsArchived,s.CreatedAt,s.UpdatedAt});}
    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> Update(Guid id,SpaceRequest request,CancellationToken ct){var s=await Owned.SingleOrDefaultAsync(s=>s.Id==id,ct);if(s is null)return NotFound();if(!new[]{"GENERAL","EXAM","LEARNING","CAREER"}.Contains(request.Type))return BadRequest(new{title="Invalid Space type."});s.Name=request.Name.Trim();s.Description=request.Description.Trim();s.Type=request.Type;s.IsArchived=request.IsArchived;s.UpdatedAt=DateTimeOffset.UtcNow;await db.SaveChangesAsync(ct);return Ok(new{s.Id,s.Name,s.Description,s.Type,s.IsArchived,s.CreatedAt,s.UpdatedAt});}
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id,CancellationToken ct){var s=await Owned.SingleOrDefaultAsync(s=>s.Id==id,ct);if(s is null)return NotFound();if(await db.MockAttempts.AnyAsync(a=>a.SpaceId==id,ct)||await db.MockTests.AnyAsync(t=>t.SpaceId==id,ct)||await db.Questions.AnyAsync(q=>q.SpaceId==id,ct)||await db.GovernmentResources.AnyAsync(r=>r.SpaceId==id,ct)||await db.Conversations.AnyAsync(c=>c.SpaceId==id,ct)||await db.Documents.AnyAsync(d=>d.SpaceId==id,ct)||await db.UserExamProfiles.AnyAsync(p=>p.SpaceId==id,ct)||await db.Goals.AnyAsync(g=>g.SpaceId==id,ct)||await db.Memories.AnyAsync(m=>m.SpaceId==id,ct)||await db.StudyPlans.AnyAsync(p=>p.SpaceId==id,ct))return Conflict(new{title="This Space contains saved work. Archive it to preserve its data."});db.Spaces.Remove(s);await db.SaveChangesAsync(ct);return NoContent();}
}
