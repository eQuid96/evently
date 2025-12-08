using System.Diagnostics.CodeAnalysis;

namespace Evently.Modules.SharedKernel;

public class Result
{
    public bool IsSuccess { get; }
    public Error Error { get; }

    public bool IsFailure => !IsSuccess;

    public Result(bool isSuccess, Error error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Failure(Error error) => new(false, error);
    public static Result Ok() => new(true, Error.None);

    public TOut Match<TOut>(Func<TOut> onSuccess, Func<Result, TOut> onError)
    {
        return IsFailure ? onError(this) : onSuccess();
    }
}


public sealed class Result<T>
{
    private readonly T? _value;
    private readonly Error[]? _errors;

    [MemberNotNullWhen(true, nameof(_value))]
    [MemberNotNullWhen(false, nameof(_errors))]
    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    private Result(T value)
    {
        _value = value;
        _errors = null;
        IsSuccess = true;
    }
    
    private Result(Error[] errors)
    {
        _value = default;
        _errors = errors;
        IsSuccess = false;
    }
    
    public T Value => IsSuccess 
        ? _value 
        : throw new InvalidOperationException("Trying to get a value a not successful result.");
    
    
    public static Result<T> Ok(T value) => new (value);
    public static Result<T> Failure(Error[] errors) => new(errors);
    public static Result<T> Failure(Error error) => new ([error]);
    
    public static implicit operator Result<T>(T? value) => 
        value is not null ? Ok(value) : Failure(Error.NullValue);

    public void Match(Action<T> onSuccess, Action<Error[]> onError)
    {
        if (IsSuccess)
        {
            onSuccess(_value);
            return;
        }
        onError(_errors);
    }
    
    public Result<TResult> Switch<TResult>(Func<T, TResult> onSuccess, Func<Error[], Error[]> onError)
    {
        if (IsSuccess)
        {
            return Result<TResult>.Ok(onSuccess(_value));
        }
        return Result<TResult>.Failure(onError(_errors));
    }

    public Result<TResult> Map<TResult>(Func<T, TResult> projection)
    {
        if (!IsSuccess)
        {
            return Result<TResult>.Failure(_errors);
        }
        return Result<TResult>.Ok(projection(_value));
    }
    
    public Result<TResult> Bind<TResult>(Func<T, Result<TResult>> projection)
    {
        if (!IsSuccess)
        {
            return Result<TResult>.Failure(_errors);
        }

        return projection(_value);
    }
    public T Unwrap()
    {
        if (!IsSuccess)
        {
            throw new InvalidOperationException("Trying to get a value a not successful result.");
        }

        return _value;
    }
    
    public Error[] GetErrors()
    {
        if (IsSuccess)
        {
            throw new InvalidOperationException("Trying to get error from successful result.");
        }

        return _errors;
    }

    public TOut Match<TOut>(Func<T, TOut> onSuccess, Func<Result<T>, TOut> onError)
    {
        if (IsFailure)
        {
            return onError(this);
        }
        
        return onSuccess(Value);

    }
}
