using Inventory.Domain.Events;
using Inventory.Domain.Interface.Data;
using Inventory.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Repositories
{
    internal class EventStoreRepository : IEventStoreRepository
    {
        private readonly EventStoreDbContext _dbContext;

        public EventStoreRepository(EventStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddEventAsync(InventoryEvent inventoryEvent)
        {
            _dbContext.InventoryEvents.Add(inventoryEvent);
            await _dbContext.SaveChangesAsync();
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
