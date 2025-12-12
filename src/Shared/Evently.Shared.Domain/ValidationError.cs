namespace Evently.Shared.Domain;

public sealed record ValidationError : Error
{
    public Error[] Errors { get; init; }

    public ValidationError(Error[] errors) 
        : base("General.Validation", "One or more validation errors occurred", ErrorType.Validation)
    {
        Errors = errors;
    }

    public static ValidationError FromResult(IEnumerable<Result> results) =>
        new([..results.Where(r => r.IsFailure).Select(r => r.Error)]);
}
