using Riktam.GroupChat.Domain.Models;

namespace Riktam.GroupChat.Domain.Services;

public interface IUserService
{
    Task<UserRecord> CreateUserAsync(CreateUserRequest model);
}
