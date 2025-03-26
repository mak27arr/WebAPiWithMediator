using Inventory.Domain.Events;
using Inventory.Domain.Interface.Repository;
using Inventory.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Repositories
{
    internal class EventStoreRepository : IEventStoreRepository
    {
        private readonly InventoryDbContext _dbContext;

        public EventStoreRepository(InventoryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddEventAsync(InventoryEvent inventoryEvent)
        {
            _dbContext.InventoryEvents.AddAsync(inventoryEvent);
        }

        public async Task<List<InventoryEvent>> GetEventsByProductIdAsync(int productId)
        {
            return await _dbContext.InventoryEvents
                .Where(e => e.ProductId == productId)
                .OrderBy(e => e.Timestamp)
                .ToListAsync();
        }
    }
}
