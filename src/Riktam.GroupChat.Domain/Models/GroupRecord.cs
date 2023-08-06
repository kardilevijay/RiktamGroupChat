namespace Riktam.GroupChat.Domain.Models;

public record GroupRecord
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
