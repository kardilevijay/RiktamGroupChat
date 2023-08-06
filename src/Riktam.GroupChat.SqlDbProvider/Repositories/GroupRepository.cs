using Microsoft.EntityFrameworkCore;
using Riktam.GroupChat.SqlDbProvider.Infrastructure;
using Riktam.GroupChat.SqlDbProvider.Models;

namespace Riktam.GroupChat.SqlDbProvider.Repositories;

internal class GroupRepository : Repository<Group>, IGroupRepository
{
    public GroupRepository(GroupChatDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Group?> GetGroupByNameAsync(string name)
    {
        return await _dbContext.Groups.FirstOrDefaultAsync(g => g.Name == name);
    }
}   
