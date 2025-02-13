using Inventory.Domain.Events;

namespace Inventory.Domain.Interface.Repository
{
    public interface IEventStoreRepository
    {
        Task AddEventAsync(InventoryEvent inventoryEvent);
        Task<List<InventoryEvent>> GetEventsByProductIdAsync(int productId);
    }
}
