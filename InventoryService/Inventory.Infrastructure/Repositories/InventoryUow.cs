using Inventory.Domain.Interface.Repository;
using Inventory.Domain.Repositories;
using Inventory.Infrastructure.Persistence;

namespace Inventory.Infrastructure.Repositories
{
    class InventoryUow : IInventoryUow
    {
        public IInventoryRepository InventoryRepository { get; protected set; }

        public IEventStoreRepository EventStoreRepository { get; protected set; }

        private InventoryDbContext _context;

        public InventoryUow(InventoryDbContext context, IInventoryRepository inventoryRepository, IEventStoreRepository eventStoreRepository)
        {
            InventoryRepository = inventoryRepository;
            EventStoreRepository = eventStoreRepository;
            _context = context;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
