using Riktam.GroupChat.Domain.Common;
using Riktam.GroupChat.Domain.Models;

namespace Riktam.GroupChat.Domain.Services;

public interface IGroupService
{
    Task<Result<GroupRecord?>> GetGroupAsync(int groupId);
    Task<Result<IEnumerable<GroupRecord>>> GetAllGroupsAsync();
    Task<Result<GroupRecord>> CreateGroupAsync(GroupRecord group);
    Task<Result<GroupRecord?>> UpdateGroupAsync(int id, GroupRecord group);
    Task<Result> DeleteGroupAsync(int groupId);
}
