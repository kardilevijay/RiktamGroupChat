namespace Riktam.GroupChat.Domain.Common;

public record Result
{
    public bool IsSuccess { get; init; }
    public AppErrorCodes ErrorCode { get; init; }
    public string ErrorMessage { get; init; } = string.Empty;

    public Result()
    {
        IsSuccess = true;
    }

    public Result(AppErrorCodes errorCode)
      : this(errorCode, errorCode.GetDescription())
    {
    }

    public Result(AppErrorCodes errorCode, string errorMessage)
    {
        IsSuccess = false;
        ErrorCode = errorCode;
        ErrorMessage = errorMessage;
    }
}

public record Result<T> : Result
{
    public T Data { get; init; } = default!;
}

public record SuccessResult<T> : Result<T>
{
    public SuccessResult(T data)
    {
        IsSuccess = true;
        Data = data;
    }
}

public record ErrorResult<T> : Result<T>
{
    public ErrorResult(AppErrorCodes errorCode)
        : this(errorCode, errorCode.GetDescription())
    {
    }

    public ErrorResult(AppErrorCodes errorCode, string errorMessage)
    {
        IsSuccess = false;
        ErrorCode = errorCode;
        ErrorMessage = errorMessage;
    }
}
