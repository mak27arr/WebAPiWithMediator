using AutoMapper;
using Inventory.Application.DTOs;
using Inventory.Application.Features.Inventory.Commands;
using Inventory.Domain.ValueObjects;

namespace Inventory.Application.Mappings
{
    internal class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ProductStoreDto, ProductStoreModel>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity));

            CreateMap<ProductStoreDto, AddProductToInventoryCommand>();
            CreateMap<ProductStoreDto, RemoveProductFromInventoryCommand>();
        }
    }
}
