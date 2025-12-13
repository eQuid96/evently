using Evently.Shared.Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Evently.Shared.Application.Pipelines;

internal sealed class RequestExceptionPipelineBehaviour<TRequest, TResponse>(
    ILogger<RequestExceptionPipelineBehaviour<TRequest, TResponse>> logger) 
    : IPipelineBehavior<TRequest, TResponse> where TRequest : class
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            return await next(cancellationToken);
        }
        catch (Exception exception)
        {
            string requestName = typeof(TRequest).Name;
            logger.LogError(exception, "Exception occurred during request: {RequestName}", requestName);
            throw  new EventlyException(requestName, innerException: exception);
        }
    }
}
