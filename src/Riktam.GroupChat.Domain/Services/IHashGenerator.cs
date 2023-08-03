namespace Riktam.GroupChat.Domain.Services;

public interface IHashGenerator
{
    string GenerateHash(string input);
}
