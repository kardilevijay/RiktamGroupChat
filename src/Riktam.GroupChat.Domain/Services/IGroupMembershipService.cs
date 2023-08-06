namespace Riktam.GroupChat.Domain.Services;

public interface IGroupMembershipService
{
    Task AddUserToGroupAsync(int userId, int groupId);
}
