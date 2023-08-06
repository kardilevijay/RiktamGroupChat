using Riktam.GroupChat.Domain.Models;
using Riktam.GroupChat.Domain.Providers;

namespace Riktam.GroupChat.Domain.Services;

public class GroupMembershipService : IGroupMembershipService
{
    private readonly IGroupMembershipProvider _groupMembershipRepository;

    public GroupMembershipService(IGroupMembershipProvider groupMembershipRepository)
    {
        _groupMembershipRepository = groupMembershipRepository;
    }

    public async Task AddUserToGroupAsync(int userId, int groupId)
    {
        var groupMembership = new GroupMembershipRecord
        {
            GroupId = groupId,
            UserId = userId
        };

        await _groupMembershipRepository.AddGroupMembershipAsync(groupMembership);
    }
}
