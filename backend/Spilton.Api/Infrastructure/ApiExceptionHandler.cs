using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
namespace Spilton.Api.Infrastructure;

public sealed class ApiExceptionHandler(ILogger<ApiExceptionHandler> logger, IProblemDetailsService problems) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken ct)
    {
        if(exception is Spilton.Api.Mocks.MockException mock){context.Response.StatusCode=mock.Status;return await problems.TryWriteAsync(new ProblemDetailsContext{HttpContext=context,ProblemDetails=new ProblemDetails{Status=mock.Status,Title=mock.Message}});}
        // Omit exception messages and request bodies, which may contain sensitive data.
        logger.LogError("Request failed: {ExceptionType}, trace {TraceId}", exception.GetType().Name, context.TraceIdentifier);
        var unavailable = exception is NpgsqlException or TimeoutException || exception.InnerException is NpgsqlException;
        context.Response.StatusCode = unavailable ? 503 : 500;
        return await problems.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = context,
            ProblemDetails = new ProblemDetails
            {
                Status = context.Response.StatusCode,
                Title = unavailable ? "Database temporarily unavailable." : "An unexpected error occurred.",
                Extensions = { ["traceId"] = context.TraceIdentifier }
            }
        });
    }
}
