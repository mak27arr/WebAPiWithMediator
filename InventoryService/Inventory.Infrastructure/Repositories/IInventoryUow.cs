using Inventory.Domain.Interface.Repository;

namespace Inventory.Infrastructure.Repositories
{
    public interface IInventory
    {
        public IInventoryRepository InventoryRepository { get; }

        public IEventStoreRepository EventStoreRepository { get; }
    }
}
