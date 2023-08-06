using Riktam.GroupChat.Domain.Common;
using Riktam.GroupChat.Domain.Models;
using Riktam.GroupChat.Domain.Providers;

namespace Riktam.GroupChat.Domain.Services;

public class GroupService : IGroupService
{
    private readonly IGroupProvider _groupProvider;

    public GroupService(IGroupProvider groupProvider)
    {
        _groupProvider = groupProvider;
    }

    public async Task<Result<GroupRecord?>> GetGroupAsync(int groupId)
    {
        var group = await _groupProvider.GetGroupAsync(groupId);
        return group is null
            ? new ErrorResult<GroupRecord?>(AppErrorCodes.NotFound)
            : new SuccessResult<GroupRecord?>(group);
    }

    public async Task<Result<IEnumerable<GroupRecord>>> GetAllGroupsAsync()
    {
        return new SuccessResult<IEnumerable<GroupRecord>>(await _groupProvider.GetAllGroupsAsync());
    }

    public async Task<Result<GroupRecord>> CreateGroupAsync(GroupRecord group)
    {
        GroupRecord? existingGroup = await _groupProvider.GetGroupByNameAsync(group.Name);
        if (existingGroup != null)
        {
            return new ErrorResult<GroupRecord>(AppErrorCodes.AlreadyExists);
        }
        return new SuccessResult<GroupRecord>(await _groupProvider.AddGroupAsync(group));
    }

    public async Task<Result<GroupRecord?>> UpdateGroupAsync(int id, GroupRecord group)
    {
        var existingGroup = await _groupProvider.GetGroupAsync(id);
        if (existingGroup == null)
        {
            return new ErrorResult<GroupRecord?>(AppErrorCodes.NotFound);
        }

        if (existingGroup.Name != group.Name)
        {
            var existingNamedGroup = await _groupProvider.GetGroupByNameAsync(group.Name);
            if (existingNamedGroup != null && existingNamedGroup.Id != group.Id)
            {
                return new ErrorResult<GroupRecord?>(AppErrorCodes.AlreadyExists);
            }
        }
        existingGroup.Name = group.Name;
        return new SuccessResult<GroupRecord?>(await _groupProvider.UpdateGroupAsync(group));
    }

    public async Task<Result> DeleteGroupAsync(int groupId)
    {
        var group = await _groupProvider.GetGroupAsync(groupId);
        if (group == null)
        {
            return new Result(AppErrorCodes.NotFound);
        }
        await _groupProvider.DeleteGroupAsync(groupId);
        return new Result();
    }
}
