using System.Security.Cryptography;
using System.Text;

namespace Riktam.GroupChat.Domain.Services;

public class SHA256HashGenerator : IHashGenerator
{
    public string GenerateHash(string input)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
        var stringBuilder = new StringBuilder();

        foreach (var b in hashedBytes)
        {
            stringBuilder.Append(b.ToString("x2"));
        }

        return stringBuilder.ToString();
    }
}
