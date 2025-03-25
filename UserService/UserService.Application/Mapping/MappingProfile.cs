using AutoMapper;
using UserService.Application.DTOs;
using UserService.Domain.Entities;

namespace Products.Core.Mapping
{
    internal class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserProfile, UserProfileDto>().ReverseMap();
        }
    }
}
