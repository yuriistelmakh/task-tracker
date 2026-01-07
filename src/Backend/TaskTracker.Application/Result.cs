namespace TaskTracker.Application;

// TODO: Rewrite every command and query so that they use result

public class Result
{
    public bool IsSuccess { get; }
    public string ErrorMessage { get; }
    public ErrorType ErrorType { get; }

    private Result(bool isSuccess, string errorMessage, ErrorType errorType)
    {
        IsSuccess = isSuccess;
        ErrorMessage = errorMessage;
        ErrorType = errorType;
    }

    public static Result Success() => new(true, string.Empty, ErrorType.None);
    public static Result Failure(string message, ErrorType type = ErrorType.Failure)
        => new(false, message, type);
    public static Result NotFound(string message) => new(false, message, ErrorType.NotFound);
    public static Result Conflict(string message) => new(false, message, ErrorType.Conflict);
}

public class Result<T>
{
    public bool IsSuccess { get; }
    public string ErrorMessage { get; }
    public ErrorType ErrorType { get; }
    public T? Value { get; }

    private Result(bool isSuccess, T? value, string errorMessage, ErrorType errorType)
    {
        IsSuccess = isSuccess;
        Value = value;
        ErrorMessage = errorMessage;
        ErrorType = errorType;
    }

    public static Result<T> Success(T value)
        => new(true, value, string.Empty, ErrorType.None);

    public static Result<T> Failure(string message, ErrorType type = ErrorType.Failure)
        => new(false, default, message, type);

    public static Result<T> NotFound(string message)
        => new(false, default, message, ErrorType.NotFound);

    public static Result<T> Conflict(string message)
        => new(false, default, message, ErrorType.Conflict);

    public static Result<T> Validation(string message)
        => new(false, default, message, ErrorType.Validation);
}

public enum ErrorType
{
    None,
    NotFound,
    Validation,
    Conflict,
    Failure
}
