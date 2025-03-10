using Inventory.Application.Features.Inventory.Commands;
using Inventory.Domain.Events;
using Inventory.Domain.Interface.Repository;
using Inventory.Domain.ValueObjects;
using MediatR;

namespace Inventory.Application.Features.Inventory.Handlers
{
    internal class RemoveProductFromInventoryCommandHandler : IRequestHandler<RemoveProductFromInventoryCommand, bool>
    {
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IEventStoreRepository _eventStoreRepository;

        public RemoveProductFromInventoryCommandHandler(IInventoryRepository inventoryRepository, IEventStoreRepository eventStoreRepository)
        {
            _inventoryRepository = inventoryRepository;
            _eventStoreRepository = eventStoreRepository;
        }

        public async Task<bool> Handle(RemoveProductFromInventoryCommand request, CancellationToken cancellationToken)
        {
            var productStoreModel = new ProductStoreModel
            {
                ProductId = request.ProductId,
                Quantity = request.Quantity
            };

            var current = await _inventoryRepository.GetProductCountAsync(productStoreModel);

            if (current < productStoreModel.Quantity)
            {
                throw new ArgumentException("Not enough in stock");
            }

            var result = await _inventoryRepository.RemoveProductFromInventoryAsync(productStoreModel);

            if (result > 0)
            {
                var inventoryEvent = new InventoryEvent
                {
                    ProductId = request.ProductId,
                    Quantity = request.Quantity,
                    Action = InventoryAction.Remove,
                    ReferenceId = request.ReferenceId,
                    ReferenceType = request.ReferenceType,
                    Timestamp = DateTime.UtcNow
                };
                await _eventStoreRepository.AddEventAsync(inventoryEvent);
            }

            return result > 0;
        }
    }
}
