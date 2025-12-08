using Evently.Modules.SharedKernel;
using Microsoft.AspNetCore.Http;

namespace Evently.Modules.Events.Presentation;

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
            type: GetProblemType(result.Error)
        );
    }
    
    public static IResult ToProblemDetail<T>(Result<T> result)
    {
        if (result.IsSuccess)
        {
            throw new InvalidOperationException("Result is not an error");
        }

        static string GetTitle(Error[] errors)
        {
            if (errors.Length == 1)
            {
                return errors[0].Code;
            }

            return "Multiple errors occurred";
        }
        
        static string GetDescription(Error[] errors)
        {
            if (errors.Length == 1)
            {
                return errors[0].Description;
            }

            return "Multiple errors occurred look at the error details";
        }
        
        static int? GetCode(Error[] errors)
        {
            if (errors.Length == 1)
            {
                return GetStatusCode(errors[0]);
            }

            return null;
        }
        
        static string? GetProblem(Error[] errors)
        {
            if (errors.Length == 1)
            {
                return GetProblemType(errors[0]);
            }

            return null;
        }
        
        static Dictionary<string, object?>? GetErrorsDetails(Error[] errors)
        {
            if (errors.Length == 1)
            {
                return null;
            }

            return new Dictionary<string, object?>
            {
                { "errorsDetails", errors }
            };
        }

        Error[] errors = result.GetErrors();
        
        return Results.Problem(
            title: GetTitle(errors),
            detail: GetDescription(errors),
            statusCode: GetCode(errors),
            type: GetProblem(errors),
            extensions: GetErrorsDetails(errors)
        );
    }
}
