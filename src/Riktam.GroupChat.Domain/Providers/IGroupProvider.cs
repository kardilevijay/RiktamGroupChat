using Riktam.GroupChat.Domain.Models;

namespace Riktam.GroupChat.Domain.Providers;

public interface IGroupProvider
{
    Task<GroupRecord> AddGroupAsync(GroupRecord group);
    Task DeleteGroupAsync(int groupId);
    Task<List<GroupRecord>> GetAllGroupsAsync();
    Task<GroupRecord?> GetGroupAsync(int groupId);
    Task<GroupRecord?> GetGroupByNameAsync(string name);
    Task<GroupRecord> UpdateGroupAsync(GroupRecord group);
}
