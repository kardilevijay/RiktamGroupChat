using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Riktam.GroupChat.SqlDbProvider.Models;

[Table("tblGroupMessages")]
public record GroupMessage
{
    [Key]
    public int Id { get; init; }

    public int GroupId { get; init; }

    public int UserId { get; init; }

    [MaxLength(500)]
    public string Message { get; init; } = string.Empty;

    public DateTime Timestamp { get; init; }

    [ForeignKey("GroupId")]
    public virtual Group Group { get; init; } = default!;

    [ForeignKey("UserId")]
    public virtual User User { get; init; } = default!;
}
