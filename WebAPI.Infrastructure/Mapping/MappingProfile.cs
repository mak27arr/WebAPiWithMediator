using AutoMapper;
using WebAPI.Core.Commands.Products;
using WebAPI.Core.Models.Products;

namespace WebAPI.Infrastructure.Mapping
{
    internal class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UpdateProductCommand, Product>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));
        }
    }
}
