using Inventory.Application.Features.Inventory.Commands;
using Inventory.Application.Interface;
using Inventory.Domain.Events;
using Inventory.Domain.Interface.Repository;
using Inventory.Domain.ValueObjects;
using MediatR;

namespace Inventory.Application.Features.Inventory.Handlers
{
    internal class AddProductToInventoryCommandHandler : IRequestHandler<AddProductToInventoryCommand, bool>
    {
        private readonly IProductGrpcService _productGrpcService;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IEventStoreRepository _eventStoreRepository;

        public AddProductToInventoryCommandHandler(IProductGrpcService productGrpcService, IInventoryRepository inventoryRepository, IEventStoreRepository eventStoreRepository)
        {
            _productGrpcService = productGrpcService;
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

            var product = await _productGrpcService.ProductExistsAsync(productStoreModel.ProductId);

            if (!product)
                return false;

            var result = await _inventoryRepository.AddProductToInventoryAsync(productStoreModel);

            if (result)
            {
                var inventoryEvent = new InventoryEvent
                {
                    ProductId = request.ProductId,
                    Quantity = request.Quantity,
                    Action = InventoryAction.Add,
                    ReferenceId = request.ReferenceId,
                    ReferenceType = request.ReferenceType,
                    Timestamp = DateTime.UtcNow
                };
                await _eventStoreRepository.AddEventAsync(inventoryEvent);
            }

            return result;
        }
    }
}
