using Inventory.Application.Interface.Services;
using Inventory.Domain.Events;

namespace Inventory.Application.Services
{
    internal class InventoryApplicationService : IInventoryApplicationService
    {
        private readonly IInventoryService _inventoryService;

        public InventoryApplicationService(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        public async Task<int?> GetProductCountAsync(int productId)
        {
            return await _inventoryService.GetProductCountAsync(productId);
        }

        public async Task HandleAddProductAsync(int productId, int amount, string addedBy, EventReferenceType addedByType)
        {
            await _inventoryService.AddProductAsync(productId, amount, addedBy, addedByType);
        }

        public async Task<int> HandleRemoveProductAsync(int productId, int amount, string addedBy, EventReferenceType addedByType)
        {
            return await _inventoryService.RemoveProductAsync(productId, amount, addedBy, addedByType);
        }

        public async Task HandleReserveProductAsync(int? productId, int amount, Guid addedBy, EventReferenceType addedByType)
        {
            await _inventoryService.ReserveProductAsync(productId, amount, addedBy, addedByType);
        }
    }

}
