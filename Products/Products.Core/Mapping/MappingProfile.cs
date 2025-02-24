using AutoMapper;
using Products.Core.Commands.Products;
using Products.Core.DTOs;
using Products.Infrastructure.Models;

namespace Products.Core.Mapping
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
