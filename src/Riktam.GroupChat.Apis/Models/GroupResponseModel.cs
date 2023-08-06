namespace Riktam.GroupChat.Apis.Models;

public record GroupResponseModel
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
}
