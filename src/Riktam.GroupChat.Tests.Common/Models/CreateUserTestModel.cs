namespace Riktam.GroupChat.Tests.Common.Models
{
    public record CreateUserTestModel
    {
        public string? UserName { get; set; }

        public string? Email { get; set; }

        public string? Password { get; set; }
    }
}
