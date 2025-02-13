using Inventory.Domain.ValueObjects;

namespace Inventory.Domain.Interface.Repository
{
    public interface IInventoryRepository
    {
        Task<bool> AddProductToInventoryAsync(ProductStoreModel productStore);
        
        Task<int?> GetProductCountAsync(ProductStoreModel productStore);
        
        Task<int> RemoveProductFromInventoryAsync(ProductStoreModel productStore);
    }
}
