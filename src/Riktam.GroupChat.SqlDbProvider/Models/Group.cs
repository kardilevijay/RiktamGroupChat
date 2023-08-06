using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Riktam.GroupChat.SqlDbProvider.Models;

[Table("tblGroups")]
public record Group
{
    [Key]
    public int Id { get; init; }

    [MaxLength(50)]
    public string Name { get; init; } = string.Empty;

    public virtual ICollection<GroupMessage> GroupMessages { get; init; } = default!;

    public virtual ICollection<GroupMembership> GroupMemberships { get; init; } = default!;
}
