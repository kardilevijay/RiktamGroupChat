using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Riktam.GroupChat.SqlDbProvider.Models;

[Table("tblUsers")]
public record User
{
    [Key]
    public int Id { get; init; }

    [MaxLength(50)]
    public string UserName { get; init; } = string.Empty;

    [MaxLength(50)]
    public string Email { get; init; } = string.Empty;

    [MaxLength(512)]
    public string Password { get; init; } = string.Empty;

    public virtual ICollection<GroupMessage> GroupMessages { get; init; } = default!;
}
