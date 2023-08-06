using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Riktam.GroupChat.SqlDbProvider.Models;

[Table("tblGroupMemberships")]
public record GroupMembership
{
    [Key]
    public int Id { get; init; }

    public int GroupId { get; init; }
    public int UserId { get; init; }

    [ForeignKey(nameof(GroupId))]
    public virtual Group Group { get; init; } = default!;

    [ForeignKey(nameof(UserId))]
    public virtual User User { get; init; } = default!;
}
