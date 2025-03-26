using Inventory.Domain.Events;

namespace Inventory.Application.Interface.Services
{
    public interface IInventoryApplicationService
    {
        Task<int?> GetProductCountAsync(int productId);
        Task HandleAddProductAsync(int productId, int amount, string addedBy, EventReferenceType addedByType);
        Task<int> HandleRemoveProductAsync(int productId, int quantity, string? referenceId, EventReferenceType referenceType);
        Task HandleReserveProductAsync(int? productId, int amount, Guid addedBy, EventReferenceType addedByType);
    }
}