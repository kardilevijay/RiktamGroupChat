using Riktam.GroupChat.Domain.Common;
using Riktam.GroupChat.Domain.Models;
using Riktam.GroupChat.Domain.Providers;

namespace Riktam.GroupChat.Domain.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IHashGenerator _hashGenerator;

    public UserService(IUserRepository userRepository, IHashGenerator hashGenerator)
    {
        _userRepository = userRepository;
        _hashGenerator = hashGenerator;
    }

    public async Task<IEnumerable<UserRecord>> GetAllUsersAsync()
    {
        var users = await _userRepository.GetAllUsersAsync();
        return users;
    }

    public async Task<UserRecord?> GetUserByIdAsync(int id)
    {
        var user = await _userRepository.GetUserByIdAsync(id);
        return user;
    }

    public async Task<UserRecord> CreateUserAsync(CreateUserRequest request)
    {
        // Check if the username is already taken
        var existingUserName = await _userRepository.GetByUserNameAsync(request.UserName);
        if (existingUserName != null)
        {
            throw new ConflictException(AppErrorCodes.UserNameAlreadyTaken);
        }

        // Check if the email is already taken
        var existingEmail = await _userRepository.GetByEmailAsync(request.Email);
        if (existingEmail != null)
        {
            throw new ConflictException(AppErrorCodes.EmailAlreadyTaken);
        }

        // Create the new user
        var newUser = new UserRecord
        {
            UserName = request.UserName,
            Email = request.Email,
            Password = _hashGenerator.GenerateHash(request.Password)
        };

        return await _userRepository.AddAsync(newUser);
    }

    public async Task<UserRecord?> UpdateUserAsync(int id, UpdateUserRequest request)
    {
        var existingUser = await _userRepository.GetUserByIdAsync(id);
        if (existingUser == null)
        {
            return null;
        }

        if (existingUser.Email != request.Email)
        {
            // Check if the email is already taken
            var existingEmailUser = await _userRepository.GetByEmailAsync(request.Email);
            if (existingEmailUser != null && existingEmailUser.Id != id)
            {
                throw new ConflictException(AppErrorCodes.EmailAlreadyTaken);
            }
            existingUser.Email = request.Email;
        }
        existingUser.Password = _hashGenerator.GenerateHash(request.Password);

        return await _userRepository.UpdateAsync(existingUser);
    }

    public async Task DeleteUserAsync(int id)
    {
        await _userRepository.DeleteAsync(id);
    }
}
