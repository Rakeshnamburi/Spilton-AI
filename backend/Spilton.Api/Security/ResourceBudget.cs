using System.Collections.Concurrent;
using System.Diagnostics;

namespace Spilton.Api.Security;

public sealed record BudgetResult(bool Allowed, TimeSpan RetryAfter);
public interface IResourceBudgetStore
{
    string Scope { get; }
    ValueTask<BudgetResult> Acquire(string key, int limit, TimeSpan window, CancellationToken ct);
}
// Replace this atomic operation with Redis INCR/expiry or a Lua transaction for
// multiple replicas. Callers must fail closed if their configured store fails.
public sealed class LocalResourceBudgetStore(TimeProvider clock) : IResourceBudgetStore
{
    private sealed class Counter { public DateTimeOffset Until; public int Count; }
    private readonly ConcurrentDictionary<string, Counter> counters = new();
    private readonly object sweepLock = new();
    public string Scope => "PROCESS_LOCAL_ONLY";
    public ValueTask<BudgetResult> Acquire(string key, int limit, TimeSpan window, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();
        var now = clock.GetUtcNow();
        // Keep the process-local store bounded. Expired keys are safe to remove;
        // active entries remain untouched and their per-entry lock serializes use.
        if (counters.Count > 10000)
        {
            lock (sweepLock)
            {
                foreach (var pair in counters)
                    if (pair.Value.Until <= now) counters.TryRemove(new KeyValuePair<string, Counter>(pair.Key, pair.Value));
                if (counters.Count > 10000) return ValueTask.FromResult(new BudgetResult(false, window));
            }
        }
        var entry = counters.GetOrAdd(key, _ => new Counter());
        lock (entry)
        {
            if (entry.Until <= now) { entry.Count = 0; entry.Until = now + window; }
            if (entry.Count >= limit) return ValueTask.FromResult(new BudgetResult(false, entry.Until - now));
            entry.Count++;
            return ValueTask.FromResult(new BudgetResult(true, TimeSpan.Zero));
        }
    }
}
public sealed class ResourceBudgetMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, IResourceBudgetStore store, ILogger<ResourceBudgetMiddleware> log)
    {
        if (context.User.Identity?.IsAuthenticated == true && context.Request.Method == "POST")
        {
            var path = context.Request.Path.Value ?? "";
            var category = path.Contains("/agents/", StringComparison.Ordinal) ? "agent" :
                path.Contains("/conversations/", StringComparison.Ordinal) && (path.EndsWith("/messages") || path.EndsWith("/regenerate")) ? "generation" :
                path == "/api/documents" ? "upload" : null;
            if (category is not null)
            {
                var limit = category == "upload" ? 10 : 20;
                var result = await store.Acquire(context.User.FindFirst("sub")!.Value + ":" + category, limit, TimeSpan.FromMinutes(1), context.RequestAborted);
                if (!result.Allowed)
                {
                    log.LogWarning("Resource budget rejected {Category}", category);
                    context.Response.StatusCode = 429;
                    context.Response.Headers.RetryAfter = Math.Max(1, (int)Math.Ceiling(result.RetryAfter.TotalSeconds)).ToString();
                    await context.Response.WriteAsJsonAsync(new { title = "Usage limit reached. Please wait and retry." });
                    return;
                }
            }
        }
        await next(context);
    }
}
