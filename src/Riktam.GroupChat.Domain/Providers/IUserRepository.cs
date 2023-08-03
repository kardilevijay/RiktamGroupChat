using Riktam.GroupChat.Domain.Models;

namespace Riktam.GroupChat.Domain.Providers;

public interface IUserRepository
{
    Task<UserRecord> AddAsync(UserRecord newUser);
    Task<UserRecord?> GetByEmailAsync(string email);
    Task<UserRecord?> GetByUserNameAsync(string username);
}
