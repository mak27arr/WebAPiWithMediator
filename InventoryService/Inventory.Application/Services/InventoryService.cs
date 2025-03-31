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

            var currentProductCount = await _inventory.InventoryRepository.GetProductAsync(productId);
            
            var isNewProduct = currentProductCount == null;
            if (isNewProduct)
                currentProductCount = new ProductInventory() { ProductId = productId };

            currentProductCount.Add(amount);
            
            if (isNewProduct) 
                await _inventory.InventoryRepository.AddProductToInventoryAsync(currentProductCount);
            else
                await _inventory.InventoryRepository.UpdateProductInInventoryAsync(currentProductCount);

            await AddProductEvent(productId, amount, InventoryAction.Add, addedBy, addedByType);
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

            await AddProductEvent(productId, amount, InventoryAction.Remove, addedBy, addedByType);
            productInventory.Remove(amount);

            await _inventory.InventoryRepository.UpdateProductInInventoryAsync(productInventory);
            await _inventory.SaveChangesAsync();

            return productInventory.Quantity.Value;
        }

        public async Task ReserveProductAsync(int? productId, int amount, Guid addedBy, EventReferenceType addedByType)
        {
            if (!productId.HasValue)
            {
                var message = "Product required";
                await SendReservationFailed(addedBy, message);
                throw new InvalidOperationException(message);
            }

            var productInventory = await _inventory.InventoryRepository.GetProductAsync(productId.Value);

            if (productInventory == null)
            {
                var message = "Product not found";
                await SendReservationFailed(addedBy, message);
                throw new InvalidOperationException(message);
            }

            productInventory.Reserve(amount);

            try
            {
                await AddProductEvent(productId.Value, amount, InventoryAction.Reserved, addedBy.ToString(), addedByType);
                await _inventory.InventoryRepository.UpdateProductInInventoryAsync(productInventory);
                await _inventory.SaveChangesAsync();
                await SendReservationSucceeded(addedBy);
            }
            catch (Exception ex)
            {
                await SendReservationFailed(addedBy, ex.Message);
            }
        }

        private async Task AddProductEvent(int productId, int amount, string action, string addedBy, EventReferenceType addedByType)
        {
            var inventoryEvent = new InventoryEvent
            {
                ProductId = productId,
                Quantity = amount,
                Action = action,
                ReferenceId = addedBy,
                ReferenceType = addedByType,
                Timestamp = DateTime.UtcNow
            };
            await _inventory.EventStoreRepository.AddEventAsync(inventoryEvent);
        }

        #region external comunication

        private async Task SendReservationSucceeded(Guid orderId)
        {
            var reservedEvent = new InventoryReservedEvent
            {
                OrderId = orderId,
            };

            await _kafkaProducer.ProduceAsync(reservedEvent);
        }

        private async Task SendReservationFailed(Guid orderId, string message)
        {
            var notAvailableEvent = new InventoryNotAvailableEvent
            {
                OrderId = orderId,
                Message = message,
            };

            await _kafkaProducer.ProduceAsync(notAvailableEvent);
        }

        private async Task CheckIfProductExists(int productId)
        {
            var product = await _productGrpcService.ProductExistsAsync(productId);

            if (!product)
                throw new InvalidOperationException($"Product {productId} does not exist.");
        }

        #endregion
    }
}
