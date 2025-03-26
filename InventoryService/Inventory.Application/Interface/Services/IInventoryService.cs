using Inventory.Domain.Events;

namespace Inventory.Application.Interface.Services
{
    public interface IInventoryService
    {
        Task AddProductAsync(int productId, int amount, string addedBy, EventReferenceType addedByType);
        Task<int?> GetProductCountAsync(int productId);
        Task<int> RemoveProductAsync(int productId, int amount, string addedBy, EventReferenceType addedByType);
        Task ReserveProductAsync(int? productId, int amount, Guid addedBy, EventReferenceType addedByType);
    }
}
