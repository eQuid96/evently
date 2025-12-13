using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Evently.Api;

internal sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    private static readonly ProblemDetails InternalServerErrorProblem = new()
    {
        Title = "Internal server error",
        Status = StatusCodes.Status500InternalServerError,
        Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1"
    };
    
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        logger.LogError(exception, "Unhandled exception occurred");

        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await httpContext.Response.WriteAsJsonAsync(InternalServerErrorProblem, cancellationToken);
        return true;
    }
}
