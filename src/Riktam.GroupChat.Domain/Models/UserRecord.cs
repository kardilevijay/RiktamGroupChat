namespace Riktam.GroupChat.Domain.Models;

public record UserRecord
{
    public int Id { get; init; }
    public string UserName { get; init; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
