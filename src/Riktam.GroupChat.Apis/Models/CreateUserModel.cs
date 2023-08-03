using System.ComponentModel.DataAnnotations;

namespace Riktam.GroupChat.Apis.Models;

public record CreateUserModel
{
    [Required]
    [MaxLength(50)]
    public string? UserName { get; init; }

    [Required]
    [EmailAddress]
    [MaxLength(50)]
    public string? Email { get; init; }

    [Required]
    [MinLength(6)]
    [MaxLength(50)]
    public string? Password { get; init; }
}
