using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Riktam.GroupChat.Apis.Models;
using Riktam.GroupChat.Domain.Models;
using Riktam.GroupChat.Domain.Services;

namespace Riktam.GroupChat.Apis.Controllers;

[ApiController]
[Route("v{version:apiVersion}/auth")]
[ApiVersion("1.0")]
public class AuthController : Controller
{
    private readonly IAuthService _userAuthService;
    private readonly IMapper _mapper;

    public AuthController(IAuthService userAuthService,
        IMapper mapper)
    {
        _userAuthService = userAuthService;
        _mapper = mapper;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] UserLoginModel model)
    {
        var authToken = await _userAuthService.LoginUserAsync(_mapper.Map<UserLoginRequest>(model));

        return Ok(new { Token = authToken });
    }
}
