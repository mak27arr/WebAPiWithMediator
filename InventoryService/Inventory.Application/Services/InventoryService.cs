using Inventory.Application.Interface.Services;
using Inventory.Domain.Entities;
using Inventory.Domain.Events;
using Inventory.Domain.Repositories;
using Products.Common.Protos.Product;
using Microsoft.Extensions.Logging;

namespace Inventory.Application.Services
{
    internal class InventoryService : IInventoryService
    {
        private readonly IInventoryUow _inventory;
        private readonly IProductGrpcService _productGrpcService;
        private readonly ILogger<InventoryService> _logger;

        public InventoryService(IInventoryUow inventory, 
            IProductGrpcService productGrpcService,  
            ILogger<InventoryService> logger)
        {
            _inventory = inventory;
            _productGrpcService = productGrpcService;
            _logger = logger;
        }

        public async Task AddProductAsync(int productId, int amount, string addedBy, EventReferenceType addedByType)
        {
            await CheckIfProductExists(productId);

            var currentProductCount = await _inventory.InventoryRepository.GetProductAsync(productId);
            var isNewProduct = currentProductCount == null;

            if (currentProductCount == null)
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

            return productInventory.Quantity ?? 0;
        }

        public async Task ReserveProductAsync(int? productId, int amount, Guid addedBy, EventReferenceType addedByType)
        {
            if (!productId.HasValue)
                throw new InvalidOperationException("Product required");

            var productInventory = await _inventory.InventoryRepository.GetProductAsync(productId.Value);

            if (productInventory == null)
            {
                var message = "Product {0} not found";
                _logger.LogInformation(message, productId);
                throw new InvalidOperationException(message);
            }

            try
            {
                productInventory.Reserve(amount);
                await AddProductEvent(productId.Value, amount, InventoryAction.Reserved, addedBy.ToString(), addedByType);
                await _inventory.InventoryRepository.UpdateProductInInventoryAsync(productInventory);
                await _inventory.SaveChangesAsync();
                _logger.LogInformation("Reserved product {0}", productId);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogInformation("Reserved fail product {0} error: {1}", productId, ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while reserving product {0} amount: {1}", productId, amount);
                throw;
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

        private async Task CheckIfProductExists(int productId)
        {
            var product = await _productGrpcService.ProductExistsAsync(productId);

            if (!product)
                throw new InvalidOperationException($"Product {productId} does not exist.");
        }

        #endregion
    }
}
