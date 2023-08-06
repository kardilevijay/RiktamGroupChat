using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Riktam.GroupChat.Apis.Models;
using Riktam.GroupChat.Domain.Models;
using Riktam.GroupChat.Domain.Services;

namespace Riktam.GroupChat.Apis.Controllers;

[ApiController]
[Route("v{version:apiVersion}/users")]
[ApiVersion("1.0")]
public class UserController : Controller
{
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public UserController(IUserService userService,
        IMapper mapper)
    {
        _userService = userService;
        _mapper = mapper;
    }

    [HttpPost]
    [ProducesResponseType(typeof(UserResponseModel), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserModel model)
    {
        var createUserRequest = _mapper.Map<CreateUserRequest>(model);
        var createdUser = await _userService.CreateUserAsync(createUserRequest);
        var createdUserResponse = _mapper.Map<UserResponseModel>(createdUser);
        return StatusCode(StatusCodes.Status201Created, createdUserResponse);
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<UserResponseModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userService.GetAllUsersAsync();
        var responseModels = _mapper.Map<IEnumerable<UserResponseModel>>(users);
        return Ok(responseModels);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(UserResponseModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserById(int id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        return user is null ? NotFound() : Ok(_mapper.Map<UserResponseModel>(user));
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserModel model)
    {
        var updateUserRequest = _mapper.Map<UpdateUserModel, UpdateUserRequest>(model);
        var updatedUser = await _userService.UpdateUserAsync(id, updateUserRequest);
        return updatedUser is null ? NotFound() : Ok(_mapper.Map<UserResponseModel>(updatedUser));
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var existingUser = await _userService.GetUserByIdAsync(id);
        if (existingUser == null)
        {
            return NotFound();
        }
        await _userService.DeleteUserAsync(id);
        return NoContent();
    }
}