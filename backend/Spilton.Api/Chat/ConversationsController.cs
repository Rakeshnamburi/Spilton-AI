using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Spilton.Api.Data;
using Spilton.Api.Documents;
using Spilton.Api.Preparation;
using Spilton.Api.Web;
namespace Spilton.Api.Chat;

public sealed record RenameRequest([Required, StringLength(100, MinimumLength = 1)] string Title);
public sealed record ConversationRequest(Guid? SpaceId=null);
public sealed record SendRequest([StringLength(8000)] string? Content, [StringLength(80)] string? Model = "auto", [StringLength(16)] string Mode = "quick", [MaxLength(5)] Guid[]? DocumentIds = null);

[ApiController, Authorize(Roles = "User"), Route("api/conversations"), RequestSizeLimit(40000), ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
public sealed class ConversationsController(AppDbContext db, IModelProviderResolver providers, ContextBuilder context,
    GenerationControl control, ModelSettings settings, CapabilityRouter capabilityRouter, CapabilityModelRouter modelRouter, IProviderHealth providerHealth, ILogger<ConversationsController> logger, RagContextBuilder rag, ScopeGuard scopes, PersonalizedContext personalization, Spilton.Api.Agents.AgentOrchestrator agent, ResearchOrchestrator research) : ControllerBase
{
    private Guid UserId => Guid.Parse(User.FindFirst("sub")!.Value);
    private IQueryable<Conversation> Owned => db.Conversations.Where(c => c.UserId == UserId);
    private static object Summary(Conversation c) => new { c.Id, c.Title, c.SpaceId, c.CreatedAt, c.UpdatedAt };

    [HttpPost, EnableRateLimiting("chat")]
    public async Task<IActionResult> Create(ConversationRequest request,CancellationToken ct)
    {
        if(!await scopes.Allowed(UserId,request.SpaceId,true,ct))return NotFound();
        var conversation = new Conversation { UserId = UserId,SpaceId=request.SpaceId };
        db.Conversations.Add(conversation); await db.SaveChangesAsync(ct);
        return Created($"/api/conversations/{conversation.Id}", Summary(conversation));
    }
    [HttpGet]
    public async Task<IActionResult> List([FromQuery] int offset = 0, [FromQuery] int limit = 50, [FromQuery]Guid? spaceId=null, CancellationToken ct = default)
    {
        if (offset < 0 || limit is < 1 or > 100) return Problem(statusCode: 400, title: "Invalid page size.");
        if(!await scopes.Allowed(UserId,spaceId,false,ct))return NotFound();
        var items = await Owned.AsNoTracking().Where(c=>c.SpaceId==spaceId).OrderByDescending(c => c.UpdatedAt).ThenByDescending(c => c.Id).Skip(offset).Take(limit + 1).ToListAsync(ct);
        return Ok(new { items = items.Take(limit).Select(Summary), hasMore = items.Count > limit });
    }
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id, [FromQuery] int? before = null, CancellationToken ct = default)
    {
        var conversation = await Owned.AsNoTracking().SingleOrDefaultAsync(c => c.Id == id, ct);
        if (conversation is null) return NotFound();
        var query = db.Messages.AsNoTracking().Where(m => m.ConversationId == id && !m.IsSuperseded);
        if (before.HasValue) query = query.Where(m => m.Sequence < before.Value);
        var messages = await query.OrderByDescending(m => m.Sequence).Take(101).ToListAsync(ct);
        return Ok(new { conversation = Summary(conversation), messages = messages.Take(100).OrderBy(m => m.Sequence).Select(MessageDto.From), hasMore = messages.Count > 100 });
    }
    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> Rename(Guid id, RenameRequest request, CancellationToken ct)
    {
        if (!await Owned.AnyAsync(c => c.Id == id, ct)) return NotFound();
        await using var gate = await ConversationLock.TryAcquire(db, id, ct);
        if (gate is null) return Problem(statusCode: 409, title: "Wait for generation to finish before changing this conversation.");
        var conversation = await Owned.SingleOrDefaultAsync(c => c.Id == id, ct);
        if (conversation is null) return NotFound();
        if (string.IsNullOrWhiteSpace(request.Title)) return Problem(statusCode: 400, title: "Enter a conversation title.");
        conversation.Title = request.Title.Trim(); conversation.UpdatedAt = DateTimeOffset.UtcNow;
        await db.SaveChangesAsync(ct); return Ok(Summary(conversation));
    }
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        if (!await Owned.AnyAsync(c => c.Id == id, ct)) return NotFound();
        await using var gate = await ConversationLock.TryAcquire(db, id, ct);
        if (gate is null) return Problem(statusCode: 409, title: "Stop generation before deleting this conversation.");
        var conversation = await Owned.SingleOrDefaultAsync(c => c.Id == id, ct);
        if (conversation is null) return NotFound();
        db.Conversations.Remove(conversation); await db.SaveChangesAsync(ct); return NoContent();
    }
    [HttpPost("{id:guid}/stop")]
    public async Task<IActionResult> Stop(Guid id, CancellationToken ct)
    {
        if (!await Owned.AnyAsync(c => c.Id == id, ct)) return NotFound();
        return Ok(new { stopped = control.Stop(id) });
    }
    [HttpPost("{id:guid}/messages"), EnableRateLimiting("chat")]
    public Task<IActionResult> Send(Guid id, SendRequest request, CancellationToken ct) => Generate(id, request, false, ct);
    [HttpPost("{id:guid}/regenerate"), EnableRateLimiting("chat")]
    public Task<IActionResult> Regenerate(Guid id, SendRequest request, CancellationToken ct) => Generate(id, request, true, ct);

    private async Task<IActionResult> Generate(Guid id, SendRequest request, bool regenerate, CancellationToken requestAborted)
    {
        if (!await Owned.AnyAsync(c => c.Id == id, requestAborted)) return NotFound();
        if (request.Mode is not ("quick" or "think" or "research" or "agent")) return Problem(statusCode: 400, title: "Choose Quick, Think, saved-source Research or Agent mode.");
        if (!regenerate && string.IsNullOrWhiteSpace(request.Content)) return Problem(statusCode: 400, title: "Enter a message.");
        IModelProvider provider;
        try { provider = providers.Resolve(request.Model); }
        catch (ProviderException ex) { return Problem(statusCode: 503, title: ex.Message); }
        await using var gate = await ConversationLock.TryAcquire(db, id, requestAborted);
        if (gate is null) return Problem(statusCode: 409, title: "A response is already being generated in this conversation.");
        var conversation = await Owned.SingleOrDefaultAsync(c => c.Id == id, requestAborted);
        if (conversation is null) return NotFound();
        if(!await scopes.Allowed(UserId,conversation.SpaceId,true,requestAborted))return Problem(statusCode:409,title:"Restore this archived Space before chatting.");
        var sequence = await db.Messages.Where(m => m.ConversationId == id).MaxAsync(m => (int?)m.Sequence, requestAborted) ?? 0;
        var recent = await db.Messages.Where(m => m.ConversationId == id && !m.IsSuperseded).OrderByDescending(m => m.Sequence).Take(50).ToListAsync(requestAborted);
        recent.Reverse();
        Message userMessage;
        Message? previousAnswer = null;
        if (regenerate) {
            userMessage = (await db.Messages.Where(m => m.ConversationId == id && m.Role == "USER").OrderByDescending(m => m.Sequence).FirstOrDefaultAsync(requestAborted))!;
            if (userMessage is null) return Problem(statusCode: 400, title: "Send a message before regenerating an answer.");
            previousAnswer = recent.LastOrDefault(m => m.Role == "ASSISTANT" && m.ReplyToId == userMessage.Id && !m.IsSuperseded);
            recent = await db.Messages.Where(m => m.ConversationId == id && !m.IsSuperseded && m.Sequence <= userMessage.Sequence)
                .OrderByDescending(m => m.Sequence).Take(50).ToListAsync(requestAborted);
            recent.Reverse();
        } else {
            userMessage = new Message { ConversationId = id, Sequence = ++sequence, Content = request.Content!.Trim(), DocumentIds = request.DocumentIds ?? [] };
            db.Messages.Add(userMessage); recent.Add(userMessage);
            if (sequence == 1 && conversation.Title == "New conversation") conversation.Title = context.Title(userMessage.Content);
        }
        var routingClock=Stopwatch.StartNew();
        var route=capabilityRouter.Route(userMessage.Content,recent,userMessage.DocumentIds.Length>0,await personalization.HasExamProfile(UserId,conversation.SpaceId,requestAborted),request.Mode);
        if(route.Capability==Capability.DOCUMENT_RAG&&userMessage.DocumentIds.Length==0)return Problem(statusCode:400,title:"Select a Ready document so Spilton can answer from its evidence.");
        provider=modelRouter.Resolve(route,request.Model);
        logger.LogInformation("Chat route {Capability} provider {Provider} model {Model} selected in {RoutingMs}ms",route.Capability,provider.Info.Provider,provider.Info.Model,routingClock.ElapsedMilliseconds);
        IReadOnlyList<ModelMessage> modelContext=context.Build(recent.Where(m=>m.DocumentIds.Length==0).ToList(),route,request.Mode);
        Citation[] citations=[];
        RagContext? researchDocuments=null;
        var advancedResearch=route.Capability==Capability.RESEARCH||request.Mode=="research";
        await using var documentLocks=new DocumentReadLocks();
        if(userMessage.DocumentIds.Length>0){try{
            await rag.Validate(UserId,userMessage.DocumentIds,requestAborted,conversation.SpaceId);
            foreach(var docId in userMessage.DocumentIds.Order()){var held=await ConversationLock.TryAcquire(db,docId,requestAborted);if(held is null)return Problem(statusCode:409,title:"A selected document is currently in use. Retry shortly.");documentLocks.Items.Add(held);}
            var grounded=await rag.Build(UserId,userMessage.DocumentIds,userMessage.Content,requestAborted,conversation.SpaceId);if(advancedResearch)researchDocuments=grounded;else{modelContext=grounded.Messages;citations=grounded.Citations;}}
            catch(DocumentException ex){return Problem(statusCode:400,title:ex.Message);}
            catch(Exception ex) when(ex is not OperationCanceledException){return Problem(statusCode:503,title:"Document retrieval is unavailable. Check the local embedding service and try again.");}}
        var personal=route.UseExamContext ? await personalization.Build(UserId,conversation.SpaceId,requestAborted) : await personalization.GeneralPreferences(UserId,conversation.SpaceId,requestAborted,userMessage.Content);
        if(personal is not null&&!advancedResearch)modelContext=[personal,..modelContext];
        var useBoundedAgent = request.Mode=="agent" && userMessage.DocumentIds.Length==0;
        var assistant = new Message { ConversationId = id, Sequence = ++sequence, Role = "ASSISTANT", Status = "generating", ModelProvider = useBoundedAgent?"Spilton Tools":provider.Info.Provider, ModelName = useBoundedAgent?"bounded-agent-v1":provider.Info.Model, ReplyToId = userMessage.Id,DocumentIds=userMessage.DocumentIds,CitationsJson=JsonSerializer.Serialize(citations.Select(c=>c with {Excerpt=""})) };
        db.Messages.Add(assistant); conversation.UpdatedAt = DateTimeOffset.UtcNow;
        await db.SaveChangesAsync(requestAborted); // Durable user input and pending response before streaming.
        using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(Math.Clamp(settings.TimeoutSeconds, 5, 120)));
        using var stopped = new CancellationTokenSource();
        using var combined = CancellationTokenSource.CreateLinkedTokenSource(requestAborted, timeout.Token, stopped.Token);
        control.Add(id, stopped);
        Response.ContentType = "text/event-stream";
        Response.Headers.CacheControl = "no-store, no-transform";
        Response.Headers["X-Accel-Buffering"] = "no";
        var output = new StringBuilder();
        var generationClock=Stopwatch.StartNew();
        string? errorCode = null, errorMessage = null;
        try {
            await Event("start", new { conversation = Summary(conversation), userMessage = regenerate ? null : MessageDto.From(userMessage), message = MessageDto.From(assistant), replacesId = previousAnswer?.Id, capability = route.Capability.ToString() }, requestAborted);
            if(advancedResearch&&!useBoundedAgent){
                var researched=await research.Run(userMessage.Content,researchDocuments,(message,progressCt)=>Event("progress",new{message},progressCt),combined.Token);
                modelContext=researched.Messages;citations=researched.Citations;
                assistant.CitationsJson=JsonSerializer.Serialize(citations.Select(c=>c with {Excerpt=""}));
                if(personal is not null)modelContext=[personal,..modelContext];
                await Event("progress",new{message="Verifying sources and citations"},combined.Token);
            }
            var lastSave = DateTimeOffset.UtcNow;
            if(useBoundedAgent){
                await Event("progress",new{message="Understanding request · creating bounded plan"},requestAborted);
                var agentResult=await agent.Run(userMessage.Content,combined.Token);
                foreach(var observation in agentResult.Observations)await Event("progress",new{message=observation.Success?$"Completed · {observation.Step}":$"Stopped · {observation.Step}"},requestAborted);
                if(agentResult.Status!="completed")throw new ProviderException("agent_failed",agentResult.Answer);
                output.Append(agentResult.Answer);await Event("delta",new{text=agentResult.Answer},requestAborted);assistant.Status="completed";
                logger.LogInformation("Agent completed {StepCount} steps for conversation {ConversationId}",agentResult.Observations.Count,id);
            } else await foreach (var delta in provider.StreamAsync(modelContext, combined.Token)) {
                combined.Token.ThrowIfCancellationRequested();
                if (output.Length + delta.Length > 32000) throw new ProviderException("response_limit", "The response reached the size limit. Ask a narrower question to continue.");
                output.Append(delta);
                await Event("delta", new { text = delta }, requestAborted);
                if (DateTimeOffset.UtcNow - lastSave > TimeSpan.FromSeconds(1)) {
                    assistant.Content = output.ToString(); await db.SaveChangesAsync(combined.Token); lastSave = DateTimeOffset.UtcNow;
                }
            }
            if (string.IsNullOrWhiteSpace(output.ToString())) throw new ProviderException("empty_response", "The model returned an empty response. Please regenerate or choose another model.");
            if(citations.Length>0&&!provider.Info.IsDevelopment){var normalized=System.Text.RegularExpressions.Regex.Replace(output.ToString(),@"[【［](\d+)[】］]","[$1]");output.Clear().Append(normalized);}
            if(citations.Length>0&&!provider.Info.IsDevelopment&&!ResearchCitationVerifier.IsValid(output.ToString(),citations))
                throw new ProviderException("invalid_citation","The model returned an unverifiable source number. Regenerate the answer and check the source excerpts.");
            assistant.Status = "completed";
            if(!useBoundedAgent)providerHealth.Success(provider.Info.Id);
        } catch (OperationCanceledException) {
            assistant.Status = timeout.IsCancellationRequested ? "failed" : "cancelled";
            errorCode = timeout.IsCancellationRequested ? "timeout" : "cancelled";
            errorMessage = timeout.IsCancellationRequested ? "Generation timed out. Your partial answer is saved; you can regenerate it." : "Generation stopped. Any partial response is saved.";
        } catch (Exception ex) {
            assistant.Status = "failed";
            errorCode = ex is ProviderException p ? p.Code : "provider_failure";
            if(!useBoundedAgent)providerHealth.Failure(provider.Info.Id,errorCode);
            errorMessage = ex is ProviderException safe ? safe.Message : "The provider connection failed. Please try again or choose another model.";
            logger.LogWarning("Generation failed: {Type}, conversation {Id}", ex.GetType().Name, id);
        } finally {
            assistant.Content = output.ToString(); conversation.UpdatedAt = DateTimeOffset.UtcNow;
            try {
                using var saveTimeout = new CancellationTokenSource(TimeSpan.FromSeconds(8));
                await using var transaction = await db.Database.BeginTransactionAsync(saveTimeout.Token);
                await db.SaveChangesAsync(saveTimeout.Token);
                if (regenerate && assistant.Status == "completed")
                    await db.Messages.Where(m => m.ConversationId == id && m.ReplyToId == userMessage.Id && m.Role == "ASSISTANT" && m.Id != assistant.Id && !m.IsSuperseded)
                        .ExecuteUpdateAsync(s => s.SetProperty(m => m.IsSuperseded, true), saveTimeout.Token);
                await transaction.CommitAsync(saveTimeout.Token);
            }
            catch { errorCode = "persistence_failed"; errorMessage = "The final response could not be saved. Reload to check the last saved content before trying again."; }
            control.Remove(id);
            logger.LogInformation("Generation finished capability {Capability} provider {Provider} model {Model} status {Status} in {GenerationMs}ms; output chars {OutputChars}, estimated tokens {EstimatedTokens}, citations {CitationCount}",route.Capability,assistant.ModelProvider,assistant.ModelName,assistant.Status,generationClock.ElapsedMilliseconds,output.Length,(output.Length+3)/4,citations.Length);
        }
        if (!requestAborted.IsCancellationRequested) {
            try {
                if (errorCode is not null) await Event("error", new { code = errorCode, message = errorMessage }, requestAborted);
                await Event("done", new { message = MessageDto.From(assistant), saved = errorCode != "persistence_failed" }, requestAborted);
            } catch (OperationCanceledException) { } catch (IOException) { }
        }
        return new EmptyResult();
    }
    private async Task Event(string type, object data, CancellationToken ct)
    {
        var json = JsonSerializer.Serialize(data, new JsonSerializerOptions(JsonSerializerDefaults.Web));
        await Response.WriteAsync($"event: {type}\ndata: {json}\n\n", ct);
        await Response.Body.FlushAsync(ct);
    }
}

internal sealed class DocumentReadLocks : IAsyncDisposable
{
    public List<ConversationLock> Items { get; }=[];
    public async ValueTask DisposeAsync(){foreach(var item in Items)await item.DisposeAsync();}
}

[ApiController, Authorize(Roles = "User"), Route("api/models")]
public sealed class ModelsController(IModelProviderResolver providers,IProviderHealth health,IMultimodalProvider multimodal,Spilton.Api.Coding.ICodeExecutionService execution) : ControllerBase
{
    [HttpGet] public IActionResult List() => Ok(new { models = providers.Available.Select(m=>new{m.Id,m.Label,m.Provider,m.Model,m.IsDevelopment,m.Capabilities,health=health.Status(m.Id)}), defaultModel = providers.DefaultId, modes = new[] { "quick", "think", "research", "agent" },vision=multimodal.Capability,codingExecution=execution.Capability });
}

