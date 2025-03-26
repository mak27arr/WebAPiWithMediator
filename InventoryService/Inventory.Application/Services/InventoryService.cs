using Inventory.Application.Interface;
using Inventory.Application.Interface.Services;
using Inventory.Domain.Entities;
using Inventory.Domain.Events;
using Inventory.Domain.Repositories;
using Products.Common.Kafka;
using Products.Common.Kafka.EventArg.Inventory;

namespace Inventory.Application.Services
{
    internal class InventoryService : IInventoryService
    {
        private readonly IInventoryUow _inventory;
        private readonly IProductGrpcService _productGrpcService;
        private readonly IKafkaProducer _kafkaProducer;

        public InventoryService(IInventoryUow inventory, IProductGrpcService productGrpcService, IKafkaProducer kafkaProducer)
        {
            _inventory = inventory;
            _productGrpcService = productGrpcService;
            _kafkaProducer = kafkaProducer;
        }

        public async Task AddProductAsync(int productId, int amount, string addedBy, EventReferenceType addedByType)
        {
            await CheckIfProductExists(productId);

            var productInventory = await _inventory.InventoryRepository.GetProductAsync(productId)
                              ?? new ProductInventory() { ProductId = productId };

            productInventory.Add(amount);
            var inventoryEvent = new InventoryEvent
            {
                ProductId = productInventory.ProductId,
                Quantity = productInventory.Quantity,
                Action = InventoryAction.Add,
                ReferenceId = addedBy,
                ReferenceType = addedByType,
                Timestamp = DateTime.UtcNow
            };

            await _inventory.InventoryRepository.UpdateProductInInventoryAsync(productInventory);
            await _inventory.EventStoreRepository.AddEventAsync(inventoryEvent);
            await _inventory.SaveChangesAsync();
        }

        public async Task<int?> GetProductCountAsync(int productId)
        {
            var productInventory = await _inventory.InventoryRepository.GetProductAsync(productId);
            return productInventory?.Quantity ?? 0;
        }

        public async Task<int> RemoveProductAsync(int productId, int amount, string addedBy, EventReferenceType addedByType)
        {
            var productInventory = await _inventory.InventoryRepository.GetProductAsync(productId);

            if (productInventory == null)
                throw new InvalidOperationException("Product not found");

            var inventoryEvent = new InventoryEvent
            {
                ProductId = productInventory.ProductId,
                Quantity = productInventory.Quantity,
                Action = InventoryAction.Remove,
                ReferenceId = addedBy,
                ReferenceType = addedByType,
                Timestamp = DateTime.UtcNow
            };
            productInventory.Remove(amount);

            await _inventory.InventoryRepository.UpdateProductInInventoryAsync(productInventory);
            await _inventory.EventStoreRepository.AddEventAsync(inventoryEvent);
            await _inventory.SaveChangesAsync();

            return productInventory.Quantity.Value;
        }

        public async Task ReserveProductAsync(int? productId, int amount, Guid addedBy, EventReferenceType addedByType)
        {
            if (!productId.HasValue)
            {
                var message = "Product required";
                SendReservationFailed(addedBy, message);
                throw new InvalidOperationException(message);
            }

            var productInventory = await _inventory.InventoryRepository.GetProductAsync(productId.Value);

            if (productInventory == null)
            {
                var message = "Product not found";
                SendReservationFailed(addedBy, message);
                throw new InvalidOperationException(message);
            }

            productInventory.Reserve(amount);

            var inventoryEvent = new InventoryEvent
            {
                ProductId = productInventory.ProductId,
                Quantity = productInventory.Quantity,
                Action = InventoryAction.Reserved,
                ReferenceId = addedBy.ToString(),
                ReferenceType = addedByType,
                Timestamp = DateTime.UtcNow
            };

            try
            {
                await _inventory.InventoryRepository.UpdateProductInInventoryAsync(productInventory);
                await _inventory.EventStoreRepository.AddEventAsync(inventoryEvent);
                await _inventory.SaveChangesAsync();
                SendReservationSucceeded(addedBy);
            }
            catch (Exception ex)
            {
                SendReservationFailed(addedBy, ex.Message);
            }
        }

        private async Task SendReservationSucceeded(Guid orderId)
        {
            var reservedEvent = new InventoryReservedEvent
            {
                OrderId = orderId,
            };

            await _kafkaProducer.ProduceAsync(KafkaInventoryTopics.InventoryReservationSucceeded, reservedEvent);
        }

        private async Task SendReservationFailed(Guid orderId, string message)
        {
            var notAvailableEvent = new InventoryNotAvailableEvent
            {
                OrderId = orderId,
                Message = message,
            };

            await _kafkaProducer.ProduceAsync(KafkaInventoryTopics.InventoryReservationFailed, notAvailableEvent);
        }

        private async Task CheckIfProductExists(int productId)
        {
            var product = await _productGrpcService.ProductExistsAsync(productId);

            if (!product)
                throw new InvalidOperationException($"Product {productId} does not exist.");
        }
    }
}
