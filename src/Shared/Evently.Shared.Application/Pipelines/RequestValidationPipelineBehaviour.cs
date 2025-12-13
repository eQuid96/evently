using System.Reflection;
using Evently.Shared.Application.Communication;
using Evently.Shared.Domain;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace Evently.Shared.Application.Pipelines;


internal sealed class RequestValidationPipelineBehaviour<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse> 
    where TRequest : IBaseCommand
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {

        ValidationFailure[] validationFailures = await ValidateAsync(request, cancellationToken);
        //No Errors just execute the request
        if (validationFailures.Length == 0)
        {
            TResponse result = await next(cancellationToken);
            return result;
        }
        //We encounter validation errors so we must return a failed Result
        Type responseType = typeof(TResponse);
        if (responseType.IsGenericType && responseType.GetGenericTypeDefinition() == typeof(Result<>))
        {
            Type genericResultType = responseType.GetGenericArguments()[0];
            
            MethodInfo? failureMethod = typeof(Result<>)
                .MakeGenericType(genericResultType)
                .GetMethod(nameof(Result<>.ValidationFailure));

            if (failureMethod is not null && failureMethod.IsStatic)
            {
                return (TResponse)failureMethod.Invoke(null, [GetValidationError(validationFailures)]);
            }
        }
        else if (responseType == typeof(Result))
        {
            return (TResponse)(object)Result.Failure(GetValidationError(validationFailures));
        }
        
        throw new ValidationException(validationFailures);
    }

    private static ValidationError GetValidationError(ValidationFailure[] failures)
    {
        Error[] errors = failures.Select(f => Error.Validation(f.ErrorCode, f.ErrorMessage)).ToArray();
        return new(errors);
    }

    private async Task<ValidationFailure[]> ValidateAsync(TRequest request, CancellationToken cancellationToken)
    {
        if (!validators.Any())
        {
            return [];
        }

        var context = new ValidationContext<TRequest>(request);
        ValidationResult[] validationResults = await Task.WhenAll(
            validators.Select(validator => validator.ValidateAsync(context, cancellationToken))
        );

        ValidationFailure[] errors = validationResults
            .Where(v => !v.IsValid)
            .SelectMany(v => v.Errors)
            .ToArray();
        
        return errors;
    }
}
