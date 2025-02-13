using AutoMapper;
using WebAPI.Core.Commands.Products;
using WebAPI.Core.DTOs;
using WebAPI.Infrastructure.Models;

namespace WebAPI.Core.Mapping
{
    internal class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDTO>().ReverseMap();
            CreateMap<Currency, CurrencyDTO>().ReverseMap();
            CreateMap<ProductPriceHistory, ProductPriceHistoryDTO>().ReverseMap();
            CreateMap<UpdateProductCommand, Product>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ReverseMap();
        }
    }
}
