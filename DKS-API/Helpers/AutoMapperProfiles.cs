using System.Linq;
using AutoMapper;
using DFPS.API.DTOs;
using DFPS.API.Models;

namespace DFPS.API.Helpers
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