namespace Riktam.GroupChat.Apis.Models;

public record UserLoginModel
{
    public string Username { get; init; }=string.Empty;
    public string Password { get; init; } = string.Empty;    
}
