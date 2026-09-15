using System.Diagnostics;

namespace Spilton.Api.Infrastructure;

public sealed class SecurityHeadersMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context)
    {
        context.Response.OnStarting(()=>
        {
            context.Response.Headers.TryAdd("X-Content-Type-Options","nosniff");
            context.Response.Headers.TryAdd("Referrer-Policy","no-referrer");
            context.Response.Headers.TryAdd("Permissions-Policy","camera=(), microphone=(), geolocation=()");
            if(!context.Response.Headers.ContainsKey("Cache-Control")&&context.Request.Path.StartsWithSegments("/api"))context.Response.Headers.CacheControl="no-store";
            return Task.CompletedTask;
        });
        await next(context);
    }
}

public sealed class RequestAuditMiddleware(RequestDelegate next,ILogger<RequestAuditMiddleware> logger)
{
    public async Task Invoke(HttpContext context)
    {
        var clock=Stopwatch.StartNew();
        try { await next(context); }
        finally
        {
            // Route templates, bodies, query values and credentials are intentionally excluded.
            logger.LogInformation("HTTP {Method} {Path} -> {Status} in {LatencyMs}ms trace {TraceId}",context.Request.Method,context.Request.Path.Value,context.Response.StatusCode,clock.ElapsedMilliseconds,context.TraceIdentifier);
        }
    }
}
