using Inventory.Domain.Interface.Repository;

namespace Inventory.Domain.Repositories
{
    public interface IInventoryUow
    {
        IInventoryRepository InventoryRepository { get; }

        IEventStoreRepository EventStoreRepository { get; }

        Task<int> SaveChangesAsync();
    }
}
