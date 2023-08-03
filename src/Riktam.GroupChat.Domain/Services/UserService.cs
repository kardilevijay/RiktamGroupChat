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

    public async Task<UserRecord> CreateUserAsync(CreateUserRequest model)
    {
        // Check if the username is already taken
        var existingUserName = await _userRepository.GetByUserNameAsync(model.UserName);
        if (existingUserName != null)
        {
            throw new ConflictException(AppErrorCodes.UserNameAlreadyTaken);
        }

        // Check if the email is already taken
        var existingEmail = await _userRepository.GetByEmailAsync(model.Email);
        if (existingEmail != null)
        {
            throw new ConflictException(AppErrorCodes.EmailAlreadyTaken);
        }

        // Create the new user
        var newUser = new UserRecord
        {
            UserName = model.UserName,
            Email = model.Email,
            Password = _hashGenerator.GenerateHash(model.Password)
        };

        return await _userRepository.AddAsync(newUser);
    }
}
