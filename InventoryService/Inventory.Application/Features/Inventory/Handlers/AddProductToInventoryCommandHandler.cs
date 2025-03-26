using Inventory.Application.Features.Inventory.Commands;
using Inventory.Application.Interface.Services;
using MediatR;

namespace Inventory.Application.Features.Inventory.Handlers
{
    internal class AddProductToInventoryCommandHandler : IRequestHandler<AddProductToInventoryCommand>
    {
        private readonly IInventoryApplicationService _inventoryApplicationService;

        public AddProductToInventoryCommandHandler(IInventoryApplicationService inventoryApplicationService)
        {
            _inventoryApplicationService = inventoryApplicationService;
        }

        public async Task Handle(AddProductToInventoryCommand request, CancellationToken cancellationToken)
        {
            await _inventoryApplicationService.HandleAddProductAsync(request.ProductId, request.Quantity, request.ReferenceId, request.ReferenceType);
        }
    }
}
