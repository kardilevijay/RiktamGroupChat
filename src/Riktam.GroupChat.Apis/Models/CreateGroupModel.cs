using System.ComponentModel.DataAnnotations;

namespace Riktam.GroupChat.Apis.Models;

public record CreateOrUpdateGroupModel
{
    [Required]
    [MaxLength(50)]
    public string? Name { get; init; }
}
