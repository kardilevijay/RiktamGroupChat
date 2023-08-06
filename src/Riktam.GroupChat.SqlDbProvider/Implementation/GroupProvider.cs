using AutoMapper;
using Riktam.GroupChat.Domain.Models;
using Riktam.GroupChat.Domain.Providers;
using Riktam.GroupChat.SqlDbProvider.Models;
using Riktam.GroupChat.SqlDbProvider.Repositories;

namespace Riktam.GroupChat.SqlDbProvider.Implementation;

internal class GroupProvider : IGroupProvider
{
    private readonly IGroupRepository _groupRepository;
    private readonly IMapper _mapper;

    public GroupProvider(IGroupRepository groupRepository, IMapper mapper)
    {
        _groupRepository = groupRepository;
        _mapper = mapper;
    }

    public async Task<GroupRecord?> GetGroupAsync(int groupId)
    {
        var group = await _groupRepository.GetByIdAsync(groupId);
        return group is null ? null : _mapper.Map<GroupRecord>(await _groupRepository.GetByIdAsync(groupId));
    }

    public async Task<GroupRecord?> GetGroupByNameAsync(string name)
    {
        var group = await _groupRepository.GetGroupByNameAsync(name);
        return group is null ? null : _mapper.Map<GroupRecord>(group);
    }

    public async Task<List<GroupRecord>> GetAllGroupsAsync()
    {
        return _mapper.Map<List<GroupRecord>>(await _groupRepository.GetAllAsync());
    }

    public async Task<GroupRecord> AddGroupAsync(GroupRecord group)
    {
        var groupEntity = _mapper.Map<Group>(group);
        await _groupRepository.AddAsync(groupEntity);
        return _mapper.Map<GroupRecord>(groupEntity);
    }

    public async Task<GroupRecord> UpdateGroupAsync(GroupRecord group)
    {
        var groupEntity = _mapper.Map<Group>(group);
        await _groupRepository.UpdateAsync(groupEntity);
        return _mapper.Map<GroupRecord>(groupEntity);
    }

    public async Task DeleteGroupAsync(int groupId)
    {
        var dbGroup = await _groupRepository.GetByIdAsync(groupId);
        if (dbGroup != null)
        {
            await _groupRepository.DeleteAsync(dbGroup);
        }
    }
}
