using AutoMapper;
using DKS.API.Models.DKSSys;
using DKS_API.DTOs;

namespace DKS_API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserForListDto>();
            CreateMap<User, UserForDetailedDto>();
            CreateMap<UserForUpdateDto, User>();
            CreateMap<User, UserDto>();
        }
    }
}