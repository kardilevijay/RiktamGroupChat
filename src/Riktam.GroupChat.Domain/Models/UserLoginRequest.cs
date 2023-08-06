namespace Riktam.GroupChat.Domain.Models;

public record UserLoginRequest
{
    public string Username { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}
