using Riktam.GroupChat.Domain.Models;

namespace Riktam.GroupChat.Domain.Providers;

public interface IGroupMembershipProvider
{
    Task AddGroupMembershipAsync(GroupMembershipRecord groupMembership);
}
