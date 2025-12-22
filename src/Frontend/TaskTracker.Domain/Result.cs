namespace TaskTracker.Domain;

public class Result<T>
{
    public bool IsSuccess { get; }
    public T? Value { get; }
    public string? ErrorMessage { get; }

    private Result(bool isSuccess, T? value, string? error)
    {
        IsSuccess = isSuccess;
        Value = value;
        ErrorMessage = error;
    }

    public static Result<T> Success(T value) => new(true, value, null);
    public static Result<T> Failure(string error) => new(false, default, error);
}

public class Result
{
    public bool IsSuccess { get; }
    public string? ErrorMessage { get; }

    protected Result(bool isSuccess, string? error)
    {
        IsSuccess = isSuccess;
        ErrorMessage = error;
    }

    public static Result Success()
    {
        return new Result(true, null);
    }

    public static Result Failure(string error)
    {
        return new Result(false, error);
    }
}
