using AutoMapper;
using graphApiService.Dtos.User;
using Microsoft.Azure.ActiveDirectory.GraphClient.Internal;

namespace graphApiService.Services
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserProfileDto>();
            CreateMap<UserProfileDto, User>();
            CreateMap<User, UserProfileCreatableDto>();
            CreateMap<UserProfileCreatableDto, User>();
            CreateMap<User, UserProfileEditableDto>();
            CreateMap<UserProfileEditableDto, User>();
        }
    }
}
