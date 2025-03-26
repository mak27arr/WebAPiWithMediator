using Inventory.Application.Features.Inventory.Commands;
using Inventory.Application.Interface.Services;
using MediatR;

namespace Inventory.Application.Features.Inventory.Handlers
{
    internal class RemoveProductFromInventoryCommandHandler : IRequestHandler<RemoveProductFromInventoryCommand, int>
    {
        private readonly IInventoryApplicationService _inventoryApplicationService;

        public RemoveProductFromInventoryCommandHandler(IInventoryApplicationService inventoryApplicationService)
        {
            _inventoryApplicationService = inventoryApplicationService;
        }

        public async Task<int> Handle(RemoveProductFromInventoryCommand request, CancellationToken cancellationToken)
        {
            return await _inventoryApplicationService.HandleRemoveProductAsync(request.ProductId, request.Quantity, request.ReferenceId, request.ReferenceType);
        }
    }
}
