using AutoMapper;
using Riktam.GroupChat.Domain.Models;
using Riktam.GroupChat.Domain.Providers;
using Riktam.GroupChat.SqlDbProvider.Models;
using Riktam.GroupChat.SqlDbProvider.Repositories;

namespace Riktam.GroupChat.SqlDbProvider.Implementation;

internal class UserProvider : IUserProvider
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UserProvider(IMapper mapper, IUserRepository userRepository)
    {
        _mapper = mapper;
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<UserRecord>> GetAllUsersAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<UserRecord>>(users);
    }

    public async Task<UserRecord?> GetUserByIdAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        return user is null ? null : _mapper.Map<UserRecord>(user);
    }

    public async Task<UserRecord?> GetByEmailAsync(string email)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        return user is null ? null : _mapper.Map<UserRecord>(user);
    }

    public async Task<UserRecord?> GetByUserNameAsync(string userName)
    {
        var user = await _userRepository.GetByUserNameAsync(userName);
        return user is null ? null : _mapper.Map<UserRecord>(user);
    }

    public async Task<UserRecord> AddAsync(UserRecord newUser)
    {
        var newDbUser = _mapper.Map<User>(newUser);
        await _userRepository.AddAsync(newDbUser);
        return _mapper.Map<UserRecord>(newDbUser);
    }

    public async Task<UserRecord?> UpdateAsync(UserRecord user)
    {
        var dbUser = await _userRepository.GetByIdAsync(user.Id);
        if (dbUser != null)
        {
            _mapper.Map(user, dbUser);
            await _userRepository.UpdateAsync(dbUser);
            return _mapper.Map<UserRecord>(dbUser);
        }
        return null;
    }

    public async Task DeleteAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user != null)
        {
            await _userRepository.DeleteAsync(user);
        }
    }
}
