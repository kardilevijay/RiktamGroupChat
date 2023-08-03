namespace Riktam.GroupChat.Domain.Common;

public class ConflictException : Exception
{
    public AppErrorCodes ErrorCode { get; }

    public ConflictException(AppErrorCodes errorCode)
    {
        ErrorCode = errorCode;
    }

    public ConflictException(AppErrorCodes errorCode, string? message) : base(message)
    {
        ErrorCode = errorCode;
    }

    public ConflictException(AppErrorCodes errorCode, string? message, Exception? innerException) : base(message, innerException)
    {
        ErrorCode = errorCode;
    }
}
