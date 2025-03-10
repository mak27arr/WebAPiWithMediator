using Inventory.Application.Features.Inventory.Commands;
using Inventory.Domain.Events;
using Inventory.Domain.Interface.Repository;
using Inventory.Domain.ValueObjects;
using MediatR;

namespace Inventory.Application.Features.Inventory.Handlers
{
    public class ReserveInventoryHandler : IRequestHandler<ReserveInventoryCommand>
    {
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IEventStoreRepository _eventStoreRepository;

        public ReserveInventoryHandler(
            IInventoryRepository inventoryRepository, 
            IEventStoreRepository eventStoreRepository)
        {
            _inventoryRepository = inventoryRepository;
            _eventStoreRepository = eventStoreRepository;
        }

        public async Task Handle(ReserveInventoryCommand request, CancellationToken cancellationToken)
        {
            //TODO: Unit of work
            var available = await _inventoryRepository.GetProductCountAsync(request.ProductRequest);

            if (available >= request.ProductRequest.Quantity)
                await ReservProduct(request);
            else
                throw new InvalidOperationException("Not enough inventory to reserve the requested quantity.");
        }

        private async Task ReservProduct(ReserveInventoryCommand request)
        {
            await _inventoryRepository.RemoveProductFromInventoryAsync(request.ProductRequest);

            var inventoryEvent = new InventoryEvent
            {
                ProductId = request.ProductRequest.ProductId,
                Quantity = request.ProductRequest.Quantity,
                Action = InventoryAction.Reserved,
                ReferenceId = request.ReferenceId,
                ReferenceType = request.ReferenceType,
                Timestamp = DateTime.UtcNow
            };
            await _eventStoreRepository.AddEventAsync(inventoryEvent);
        }
    }
}
