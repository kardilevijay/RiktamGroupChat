using Microsoft.AspNetCore.Mvc;
using Riktam.GroupChat.Apis.Models;
using Riktam.GroupChat.Domain.Models;
using Riktam.GroupChat.Domain.Providers;
using Riktam.GroupChat.Domain.Services;

namespace Riktam.GroupChat.Apis.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : Controller
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(UserResponseModel), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserModel model)
    {
        var newUser = await _userService.CreateUserAsync(Map(model));
        return StatusCode(StatusCodes.Status201Created, Map(newUser));
    }

    private static UserResponseModel Map(UserRecord newUser)
    {
        return new UserResponseModel
        {
            Id = newUser.Id,
            UserName = newUser.UserName,
            Email = newUser.Email
        };
    }

    private static CreateUserRequest Map(CreateUserModel model)
    {
        return new CreateUserRequest
        {
            UserName = model.UserName!,
            Email = model.Email!,
            Password = model.Password!
        };
    }
}
