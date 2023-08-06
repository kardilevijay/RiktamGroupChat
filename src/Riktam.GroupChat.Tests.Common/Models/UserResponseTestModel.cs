namespace Riktam.GroupChat.Tests.Common.Models
{
    public record UserResponseTestModel
    {
        public int Id { get; init; }
        public string UserName { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
    }
}
