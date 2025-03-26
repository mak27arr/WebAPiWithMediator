using AutoMapper;
using Inventory.Domain.Entities;
using Inventory.Infrastructure.Entity;

namespace Inventory.Infrastructure.Mappings
{
    internal class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ProductInventory, ProductInventoryEntity>().ReverseMap();
        }
    }
}
