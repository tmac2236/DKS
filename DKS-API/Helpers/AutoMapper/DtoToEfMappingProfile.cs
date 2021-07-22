using AutoMapper;
using DKS.API.Models.DKS;
using DKS_API.DTOs;

namespace DKS_API.Helpers.AutoMapper
{
    public class DtoToEfMappingProfile : Profile
    {
        public DtoToEfMappingProfile()
        {
            CreateMap<AddDevDtrFgtResultDto, DevDtrFgtResult>();
        }
    }
}