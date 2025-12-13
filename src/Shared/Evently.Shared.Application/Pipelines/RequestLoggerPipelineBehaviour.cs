using Evently.Shared.Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace Evently.Shared.Application.Pipelines;

internal sealed class RequestLoggerPipelineBehaviour<TRequest, TResponse>(ILogger<RequestLoggerPipelineBehaviour<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse> 
    where TRequest : class
    where TResponse : Result
{
    public async Task<TResponse> Handle(
        TRequest request, 
        RequestHandlerDelegate<TResponse> next, 
        CancellationToken cancellationToken)
    {
        string requestName = typeof(TRequest).Name;
        logger.LogInformation("Processing request: {RequestName}", requestName);
        TResponse result = await next(cancellationToken);
        
        using (LogContext.PushProperty("Module", GetModuleName(typeof(TRequest).FullName!)))
        {
            if (result.IsFailure)
            {
                using (LogContext.PushProperty("Error", result.Error, destructureObjects: true))
                {
                    logger.LogError("Error on request: {RequestName}", requestName);
                }
            }
            else
            {
                logger.LogInformation("Completed request: {RequestName}", requestName);
            }
        }
        return result;
    }

    private static string GetModuleName(string requestFullName)
    {
        Span<string> parts = requestFullName.Split('.');
        if (parts.Length > 2)
        {
            return parts[2];
        }
        return string.Empty;
    }
}
