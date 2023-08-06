using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Riktam.GroupChat.Domain.Models;
using Riktam.GroupChat.Domain.Providers;
using Riktam.GroupChat.SqlDbProvider.Infrastructure;
using Riktam.GroupChat.SqlDbProvider.Models;

namespace Riktam.GroupChat.SqlDbProvider.Implementation;

internal class UserRepository : IUserRepository
{
    private readonly GroupChatDbContext _dbContext;
    private readonly IMapper _mapper;

    public UserRepository(GroupChatDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<IEnumerable<UserRecord>> GetAllUsersAsync()
    {
        var users = await _dbContext.Users.AsNoTracking().ToListAsync();
        return _mapper.Map<IEnumerable<UserRecord>>(users);
    }

    public async Task<UserRecord?> GetUserByIdAsync(int id)
    {
        var user = await _dbContext.Users.FindAsync(id);
        return user is null ? null : _mapper.Map<UserRecord>(user);
    }

    public async Task<UserRecord?> GetByEmailAsync(string email)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
        return user is null ? null : _mapper.Map<UserRecord>(user);
    }

    public async Task<UserRecord?> GetByUserNameAsync(string userName)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserName == userName);
        return user is null ? null : _mapper.Map<UserRecord>(user);
    }

    public async Task<UserRecord> AddAsync(UserRecord newUser)
    {
        var newDbUser = _mapper.Map<User>(newUser);
        await _dbContext.Users.AddAsync(newDbUser);
        await _dbContext.SaveChangesAsync();
        return _mapper.Map<UserRecord>(newDbUser);
    }

    public async Task<UserRecord?> UpdateAsync(UserRecord user)
    {
        var dbUser = await _dbContext.Users.FindAsync(user.Id);
        if (dbUser != null)
        {
            _mapper.Map(user, dbUser);
            await _dbContext.SaveChangesAsync();
            return _mapper.Map<UserRecord>(dbUser);
        }
        return null;
    }

    public async Task DeleteAsync(int id)
    {
        var user = await _dbContext.Users.FindAsync(id);
        if (user != null)
        {
            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();
        }
    }
}
