namespace Riktam.GroupChat.Domain.Models;

public record GroupRecord
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
}
