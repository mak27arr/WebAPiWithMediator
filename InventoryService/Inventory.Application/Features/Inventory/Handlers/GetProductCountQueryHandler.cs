using Inventory.Application.Features.Inventory.Queries;
using Inventory.Application.Interface.Services;
using MediatR;

namespace Inventory.Application.Features.Inventory.Handlers
{
    internal class GetProductCountQueryHandler : IRequestHandler<GetProductCountQuery, int?>
    {
        private readonly IInventoryApplicationService _inventoryApplicationService;

        public GetProductCountQueryHandler(IInventoryApplicationService inventoryApplicationService)
        {
            _inventoryApplicationService = inventoryApplicationService;
        }

        public async Task<int?> Handle(GetProductCountQuery request, CancellationToken cancellationToken)
        {
            return await _inventoryApplicationService.GetProductCountAsync(request.ProductId);
        }
    }
}
