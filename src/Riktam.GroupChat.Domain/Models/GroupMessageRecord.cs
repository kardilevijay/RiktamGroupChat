namespace Riktam.GroupChat.Domain.Models;

public record GroupMessageRecord
{
    public int Id { get; init; }
    public int GroupId { get; init; }
    public int UserId { get; init; }
    public string Message { get; init; } = string.Empty;
    public DateTime Timestamp { get; init; }

    public GroupRecord Group { get; init; } = default!;
    public UserRecord User { get; init; } = default!;
}
