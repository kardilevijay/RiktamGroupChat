using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Riktam.GroupChat.Apis.Models;
using Riktam.GroupChat.Domain.Common;
using Riktam.GroupChat.Domain.Models;
using Riktam.GroupChat.Domain.Services;

namespace Riktam.GroupChat.Apis.Controllers;

[ApiController]
[Route("v{version:apiVersion}/groups")]
[ApiVersion("1.0")]
public class GroupController : ControllerBase
{
    private readonly IGroupService _groupService;
    private readonly IMapper _mapper;

    public GroupController(IGroupService groupService, IMapper mapper)
    {
        _groupService = groupService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetGroups()
    {
        var result = await _groupService.GetAllGroupsAsync();
        if (result.IsSuccess == false)
        {
            var responseModels = _mapper.Map<List<GroupResponseModel>>(result.Data);
            return Ok(responseModels);
        }

        return HandleErrorResponse(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetGroup(int id)
    {
        var result = await _groupService.GetGroupAsync(id);
        if (result.IsSuccess)
        {
            return Ok(_mapper.Map<GroupResponseModel>(result.Data));
        }

        return HandleErrorResponse(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(GroupResponseModel), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateGroup([FromBody] CreateOrUpdateGroupModel model)
    {
        var result = await _groupService.CreateGroupAsync(_mapper.Map<GroupRecord>(model));

        if (result.IsSuccess)
        {
            var responseModel = _mapper.Map<GroupResponseModel>(result.Data);
            return CreatedAtAction(nameof(GetGroup), new { id = result.Data.Id }, responseModel);
        }

        return HandleErrorResponse(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateGroup(int id, [FromBody] CreateOrUpdateGroupModel model)
    {
        var result = await _groupService.UpdateGroupAsync(id, _mapper.Map<GroupRecord>(model));
        if (result.IsSuccess)
        {
            return NoContent();
        }

        return HandleErrorResponse(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGroup(int id)
    {
        var result = await _groupService.DeleteGroupAsync(id);
        if (result.IsSuccess)
        {
            return NoContent();
        }

        return HandleErrorResponse(result);
    }

    private IActionResult HandleErrorResponse(Result result)
    {
        var errorResponseModel = new ErrorResponseModel
        {
            ErrorCode = result.ErrorCode,
            ErrorMessage = result.ErrorMessage
        };

        if (result.ErrorCode == AppErrorCodes.NotFound)
        {
            return NotFound(errorResponseModel);
        }

        if (result.ErrorCode == AppErrorCodes.AlreadyExists)
        {
            return Conflict(errorResponseModel);
        }

        return StatusCode(StatusCodes.Status500InternalServerError, errorResponseModel);
    }
}