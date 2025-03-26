using AutoMapper;
using Inventory.Domain.Entities;
using Inventory.Domain.Interface.Repository;
using Inventory.Infrastructure.Entity;
using Inventory.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Repositories
{
    internal class InventoryRepository : IInventoryRepository
    {
        private readonly InventoryDbContext _dbContext;
        private readonly IMapper _mapper;

        public InventoryRepository(InventoryDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<bool> AddProductToInventoryAsync(ProductInventory productInventory)
        {
            var productEntity = _mapper.Map<ProductInventoryEntity>(productInventory);
            await _dbContext.ProductInventories.AddAsync(productEntity);
            return true;
        }

        public async Task<ProductInventory> GetProductAsync(int productId)
        {
            var productInventory = await _dbContext.ProductInventories
                .Where(p => p.ProductId == productId)
                .FirstOrDefaultAsync();

            return _mapper.Map<ProductInventory>(productInventory);
        }

        public async Task<int> UpdateProductInInventoryAsync(ProductInventory productInventory)
        {
            var currentProductInventory = await _dbContext.ProductInventories
                .Where(p => p.ProductId == productInventory.ProductId)
                .FirstOrDefaultAsync();

            if (currentProductInventory == null)
                throw new KeyNotFoundException($"{productInventory.ProductId}");

            var productEntity = _mapper.Map<ProductInventoryEntity>(productInventory);
            _dbContext.ProductInventories.Attach(productEntity);
            _dbContext.Entry(productEntity).State = EntityState.Modified;

            return 0;
        }
    }
}
