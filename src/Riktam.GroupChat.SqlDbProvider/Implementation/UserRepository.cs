using Microsoft.EntityFrameworkCore;
using Riktam.GroupChat.Domain.Models;
using Riktam.GroupChat.Domain.Providers;
using Riktam.GroupChat.SqlDbProvider.Infrastructure;
using Riktam.GroupChat.SqlDbProvider.Models;

namespace Riktam.GroupChat.SqlDbProvider.Implementation;

internal class UserRepository : IUserRepository
{
    private readonly GroupChatDbContext _dbContext;

    public UserRepository(GroupChatDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<UserRecord> AddAsync(UserRecord newUser)
    {
        var newDbUser = Map(newUser);
        await _dbContext.Users.AddAsync(newDbUser);
        await _dbContext.SaveChangesAsync();
        return Map(newDbUser);
    }

    public async Task<UserRecord?> GetByEmailAsync(string email)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
        return user is null ? null : Map(user);
    }

    public async Task<UserRecord?> GetByUserNameAsync(string userName)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserName == userName);
        return user is null ? null : Map(user);
    }

    private static UserRecord Map(User user)
    {
        return new UserRecord
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            Password = user.Password
        };
    }
    private static User Map(UserRecord newUser)
    {
        return new User
        {
            UserName = newUser.UserName,
            Email = newUser.Email,
            Password = newUser.Password
        };
    }
}
