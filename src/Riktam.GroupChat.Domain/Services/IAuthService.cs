using Riktam.GroupChat.Domain.Models;

namespace Riktam.GroupChat.Domain.Services;

public interface IAuthService
{
    Task<string?> LoginUserAsync(UserLoginRequest model);
}