
using Inventory.Domain.Entities;

namespace Inventory.Domain.Interface.Repository
{
    public interface IInventoryRepository
    {
        Task<bool> AddProductToInventoryAsync(ProductInventory productStore);

        Task<ProductInventory> GetProductAsync(int productId);

        Task<int> UpdateProductInInventoryAsync(ProductInventory productStore);
    }
}
