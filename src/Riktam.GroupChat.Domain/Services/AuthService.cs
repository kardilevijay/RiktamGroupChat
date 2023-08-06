using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Riktam.GroupChat.Domain.Models;
using Riktam.GroupChat.Domain.Providers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Riktam.GroupChat.Domain.Services;

public class AuthService : IAuthService
{
    private readonly IUserProvider _userRepository;
    private readonly IHashGenerator _hashGenerator;
    private readonly IConfiguration _configuration;

    public AuthService(IUserProvider userRepository, IHashGenerator hashGenerator, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _hashGenerator = hashGenerator;
        _configuration = configuration;
    }

    public async Task<string?> LoginUserAsync(UserLoginRequest model)
    {
        var user = await _userRepository.GetByUserNameAsync(model.Username) ?? throw new UnauthorizedAccessException();
        var hashedPassword = _hashGenerator.GenerateHash(model.Password);

        if (user.Password != hashedPassword)
        {
            throw new UnauthorizedAccessException();
        }

        return GenerateJwtToken(user);
    }

    private string GenerateJwtToken(UserRecord user)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");

        var key = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]
            ?? throw new ApplicationException("JwtSettings.SecretKey not defined"));

        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("UserId", user.Id.ToString())
            }),
            Audience = jwtSettings["Audience"],
            Issuer = jwtSettings["Issuer"],
            Expires = DateTime.UtcNow.AddMinutes(int.Parse(jwtSettings["ExpirationMinutes"]
                ?? throw new ApplicationException("JwtSettings.ExpirationMinutes not defined"))),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}