using Evently.Shared.Domain;
using Microsoft.AspNetCore.Http;

namespace Evently.Shared.Presentation;

public static class ApiResults
{
    private static int GetStatusCode(Error error)
    {
        return error.Type switch
        {
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Problem => StatusCodes.Status400BadRequest,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.Failure => StatusCodes.Status500InternalServerError,
            _ => StatusCodes.Status500InternalServerError
        };
    }

    private static string GetProblemType(Error error)
    {
        return error.Type switch
        {
            ErrorType.Validation => "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            ErrorType.Problem => "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            ErrorType.NotFound => "https://tools.ietf.org/html/rfc7231#section-6.5.4",
            _ => "https://tools.ietf.org/html/rfc7231#section-6.6.1"
        };
    }
    
    private static Dictionary<string, object?>? GetErrorsDetails(Error error)
    {
        if (error is ValidationError validationError)
        {
            return new Dictionary<string, object?>
            {
                { "errors", validationError.Errors }
            };
        }
        return null;
    }
    
    public static IResult ToProblemDetail(Result result)
    {
        if (result.IsSuccess)
        {
            throw new InvalidOperationException("Result is not an error");
        }
        
        return Results.Problem(
            title: result.Error.Code,
            detail: result.Error.Description,
            statusCode: GetStatusCode(result.Error),
            type: GetProblemType(result.Error),
            extensions: GetErrorsDetails(result.Error)
        );
    }
}
