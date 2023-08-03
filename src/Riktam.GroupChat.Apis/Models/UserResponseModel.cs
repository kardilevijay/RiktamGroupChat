namespace Riktam.GroupChat.Apis.Models;

public record UserResponseModel
{
    public int Id { get; init; }
    public string UserName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
}
