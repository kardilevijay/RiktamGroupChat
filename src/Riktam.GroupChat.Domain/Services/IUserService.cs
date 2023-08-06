using Riktam.GroupChat.Domain.Models;

namespace Riktam.GroupChat.Domain.Services;

public interface IUserService
{
    Task<IEnumerable<UserRecord>> GetAllUsersAsync();
    Task<UserRecord?> GetUserByIdAsync(int id);
    Task<UserRecord> CreateUserAsync(CreateUserRequest request);
    Task<UserRecord?> UpdateUserAsync(int id, UpdateUserRequest request);
    Task DeleteUserAsync(int id);
}