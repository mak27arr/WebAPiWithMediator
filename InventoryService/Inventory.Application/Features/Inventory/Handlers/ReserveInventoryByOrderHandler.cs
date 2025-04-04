using FluentResults;
using Inventory.Application.Features.Inventory.Commands;
using Inventory.Application.Interface.Services;
using Inventory.Domain.Events;
using MediatR;

namespace Inventory.Application.Features.Inventory.Handlers
{
    internal class ReserveInventoryByOrderHandler : IRequestHandler<ReserveInventoryByOrderCommand>
    {
        private IInventoryApplicationService _inventoryApplicationService;

        public ReserveInventoryByOrderHandler(IInventoryApplicationService inventoryApplicationService)
        {
            _inventoryApplicationService = inventoryApplicationService;
        }

        public async Task Handle(ReserveInventoryByOrderCommand request, CancellationToken cancellationToken)
        {
            await _inventoryApplicationService.HandleReserveProductAsync(request.Product,
                request.Quantity,
                request.OrderId,
                EventReferenceType.Order);
        }
    }
}
