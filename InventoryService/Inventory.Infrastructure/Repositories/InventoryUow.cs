using Inventory.Domain.Interface.Repository;
using Inventory.Domain.Repositories;
using Inventory.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Inventory.Infrastructure.Repositories
{
    class InventoryUow : IInventoryUow
    {
        public IInventoryRepository InventoryRepository { get; protected set; }

        public IEventStoreRepository EventStoreRepository { get; protected set; }

        private InventoryDbContext _context;

        private readonly ILogger<InventoryUow> _logger;

        public InventoryUow(InventoryDbContext context, IInventoryRepository inventoryRepository, IEventStoreRepository eventStoreRepository, ILogger<InventoryUow> logger)
        {
            InventoryRepository = inventoryRepository;
            EventStoreRepository = eventStoreRepository;
            _context = context;
            _logger = logger;
        }

        public async Task<int> SaveChangesAsync()
        {
            try
            {
                return await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError($"Error: {ex.InnerException?.Message}");
                throw;
            }
        }
    }
}
