using AutoMapper;
using Riktam.GroupChat.Apis.Models;
using Riktam.GroupChat.Domain.Models;

namespace Riktam.GroupChat.Apis.Mapper;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<CreateUserModel, CreateUserRequest>().ReverseMap();
        CreateMap<UserRecord, UserResponseModel>();
        CreateMap<UserLoginModel, UserLoginRequest>().ReverseMap();
    }
}
