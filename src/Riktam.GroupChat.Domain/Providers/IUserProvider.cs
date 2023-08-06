using Riktam.GroupChat.Domain.Models;

namespace Riktam.GroupChat.Domain.Providers;

public interface IUserProvider
{
    Task<IEnumerable<UserRecord>> GetAllUsersAsync();
    Task<UserRecord?> GetByEmailAsync(string email);
    Task<UserRecord?> GetByUserNameAsync(string username);
    Task<UserRecord?> GetUserByIdAsync(int id);
    Task<UserRecord> AddAsync(UserRecord newUser);
    Task DeleteAsync(int id);
    Task<UserRecord?> UpdateAsync(UserRecord user);
}
