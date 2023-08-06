namespace Riktam.GroupChat.Domain.Common;

public class ClientException : Exception
{
    public AppErrorCodes ErrorCode { get; }

    public ClientException(AppErrorCodes errorCode)
    {
        ErrorCode = errorCode;
    }

    public ClientException(AppErrorCodes errorCode, string? message) : base(message)
    {
        ErrorCode = errorCode;
    }

    public ClientException(AppErrorCodes errorCode, string? message, Exception? innerException) : base(message, innerException)
    {
        ErrorCode = errorCode;
    }
}