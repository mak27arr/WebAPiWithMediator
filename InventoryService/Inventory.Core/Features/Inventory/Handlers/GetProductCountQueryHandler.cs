using Inventory.Application.Features.Inventory.Queries;
using Inventory.Application.Interface.Repository;
using Inventory.Domain.ValueObjects;
using MediatR;

namespace Inventory.Application.Features.Inventory.Handlers
{
    internal class GetProductCountQueryHandler : IRequestHandler<GetProductCountQuery, int?>
    {
        private readonly IInventoryRepository _inventoryRepository;

        public GetProductCountQueryHandler(IInventoryRepository inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
        }

        public async Task<int?> Handle(GetProductCountQuery request, CancellationToken cancellationToken)
        {
            var productStoreModel = new ProductStoreModel
            {
                ProductId = request.ProductId,
            };

            return await _inventoryRepository.GetProductCountAsync(productStoreModel);
        }
    }
}
