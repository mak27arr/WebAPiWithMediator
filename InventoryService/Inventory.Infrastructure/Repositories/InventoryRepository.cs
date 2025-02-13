using Inventory.Domain.Entities;
using Inventory.Domain.Interface.Repository;
using Inventory.Domain.ValueObjects;
using Inventory.Infrastructure.Persistence;

namespace Inventory.Infrastructure.Repositories
{
    internal class InventoryRepository : IInventoryRepository
    {
        private readonly InventoryDbContext _dbContext;

        public InventoryRepository(InventoryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> AddProductToInventoryAsync(ProductStoreModel productStore)
        {
            var productInventory = new ProductInventory(productStore.ProductId, productStore.Quantity);

            _dbContext.ProductInventories.Add(productInventory);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<int?> GetProductCountAsync(ProductStoreModel productStore)
        {
            var productInventory = await _dbContext.ProductInventories
                .Where(p => p.ProductId == productStore.ProductId && p.RoomId == productStore.RoomId)
                .FirstOrDefaultAsync();

            return productInventory?.Quantity;
        }

        public async Task<int> RemoveProductFromInventoryAsync(ProductStoreModel productStore)
        {
            var productInventory = await _dbContext.ProductInventories
                .Where(p => p.ProductId == productStore.ProductId && p.RoomId == productStore.RoomId)
                .FirstOrDefaultAsync();

            if (productInventory != null)
            {
                productInventory.Quantity -= productStore.Quantity;
                if (productInventory.Quantity <= 0)
                {
                    _dbContext.ProductInventories.Remove(productInventory);
                }
                await _dbContext.SaveChangesAsync();
                return productInventory.Quantity;
            }

            return 0;
        }
    }
}
