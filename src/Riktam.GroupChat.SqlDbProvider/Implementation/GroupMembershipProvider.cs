using AutoMapper;
using Riktam.GroupChat.Domain.Models;
using Riktam.GroupChat.Domain.Providers;
using Riktam.GroupChat.SqlDbProvider.Infrastructure;
using Riktam.GroupChat.SqlDbProvider.Models;

namespace Riktam.GroupChat.SqlDbProvider.Implementation;

internal class GroupMembershipProvider : IGroupMembershipProvider
{
    private readonly GroupChatDbContext _dbContext;
    private readonly IMapper _mapper;

    public GroupMembershipProvider(GroupChatDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task AddGroupMembershipAsync(GroupMembershipRecord groupMembership)
    {
        await _dbContext.GroupMemberships.AddAsync(_mapper.Map<GroupMembership>(groupMembership));
        await _dbContext.SaveChangesAsync();
    }
}
