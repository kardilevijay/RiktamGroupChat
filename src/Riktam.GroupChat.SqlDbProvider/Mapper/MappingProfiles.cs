using AutoMapper;
using Riktam.GroupChat.Domain.Models;
using Riktam.GroupChat.SqlDbProvider.Models;

namespace Riktam.GroupChat.SqlDbProvider.Mapper;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<User, UserRecord>().ReverseMap();
    }
}
