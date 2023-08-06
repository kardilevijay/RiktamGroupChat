namespace Riktam.GroupChat.Domain.Models;

public record GroupMembershipRecord
{
    public int Id { get; init; }
    public int GroupId { get; init; }
    public int UserId { get; init; }
}
