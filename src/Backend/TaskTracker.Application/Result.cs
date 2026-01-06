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

public enum ErrorType
{
    None,
    NotFound,
    Validation,
    Conflict,
    Failure
}
