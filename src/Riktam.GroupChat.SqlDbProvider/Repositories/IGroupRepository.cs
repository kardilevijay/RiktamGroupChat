using Riktam.GroupChat.SqlDbProvider.Infrastructure;
using Riktam.GroupChat.SqlDbProvider.Models;

namespace Riktam.GroupChat.SqlDbProvider.Repositories;

internal interface IGroupRepository : IRepository<Group>
{
    Task<Group?> GetGroupByNameAsync(string name);
}
