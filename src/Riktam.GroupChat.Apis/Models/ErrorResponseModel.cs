using Riktam.GroupChat.Domain.Common;

namespace Riktam.GroupChat.Apis.Models;

public record ErrorResponseModel
{
    public AppErrorCodes ErrorCode { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
}
