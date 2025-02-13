using Inventory.Application.Features.Inventory.Commands;
using Inventory.Application.Interface.Data;
using Inventory.Application.Interface.Repository;
using Inventory.Domain.Events;
using Inventory.Domain.ValueObjects;
using MediatR;

namespace Inventory.Application.Features.Inventory.Handlers
{
    internal class AddProductToInventoryCommandHandler : IRequestHandler<AddProductToInventoryCommand, bool>
    {
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IEventStoreRepository _eventStoreRepository;

        public AddProductToInventoryCommandHandler(IInventoryRepository inventoryRepository, IEventStoreRepository eventStoreRepository)
        {
            _inventoryRepository = inventoryRepository;
            _eventStoreRepository = eventStoreRepository;
        }

        public async Task<bool> Handle(AddProductToInventoryCommand request, CancellationToken cancellationToken)
        {
            var productStoreModel = new ProductStoreModel
            {
                ProductId = request.ProductId,
                Quantity = request.Quantity
            };

            var result = await _inventoryRepository.AddProductToInventoryAsync(productStoreModel);

            if (result)
            {
                var inventoryEvent = new InventoryEvent
                {
                    ProductId = request.ProductId,
                    Quantity = request.Quantity,
                    Action = "Add",
                    Timestamp = DateTime.UtcNow
                };
                await _eventStoreRepository.AddEventAsync(inventoryEvent);
            }

            return result;
        }
    }
}
