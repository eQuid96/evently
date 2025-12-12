namespace Evently.Shared.Domain;

public class Result
{
    public Error Error { get; }
    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    protected Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None ||
            !isSuccess && error == Error.None)
        {
            throw new ArgumentException("Error creating a result object with the error", nameof(error));
        }
        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Failure(Error error) => new(false, error);
    public static Result<TValue> Failure<TValue>(Error error) => new(false, error, default);
    public static Result Ok() => new(true, Error.None);
    public TOut Match<TOut>(Func<TOut> onSuccess, Func<Result, TOut> onError)
    {
        return IsFailure ? onError(this) : onSuccess();
    }
}


public class Result<T> : Result
{
    private readonly T? _value;
    public Result(bool isSuccess, Error error, T? value) : base(isSuccess, error)
    {
        _value = value;
    }

    public T Value => IsSuccess 
        ? _value 
        : throw new InvalidOperationException("Trying to get a value a not successful result.");
    
    
    public static Result<T> Ok(T value) => new (true, Error.None, value);
    
    public static implicit operator Result<T>(T? value) => 
        value is not null ? Ok(value) : Failure<T>(Error.NullValue);

    public void Match(Action<T> onSuccess, Action<Error> onError)
    {
        if (IsSuccess)
        {
            onSuccess(Value);
            return;
        }
        onError(Error);
    }
    
    public Result<TResult> Switch<TResult>(Func<T, TResult> onSuccess, Func<Error, Error> onError)
    {
        if (IsSuccess)
        {
            return Result<TResult>.Ok(onSuccess(Value));
        }
        return Failure<TResult>(onError(Error));
    }

    public Result<TResult> Map<TResult>(Func<T, TResult> projection)
    {
        if (!IsSuccess)
        {
            return Failure<TResult>(Error);
        }
        return Result<TResult>.Ok(projection(Value));
    }
    
    public Result<TResult> Bind<TResult>(Func<T, Result<TResult>> projection)
    {
        if (!IsSuccess)
        {
            return Failure<TResult>(Error);
        }

        return projection(Value);
    }
    public T Unwrap()
    {
        if (!IsSuccess)
        {
            throw new InvalidOperationException("Trying to get a value a not successful result.");
        }
        return _value;
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
