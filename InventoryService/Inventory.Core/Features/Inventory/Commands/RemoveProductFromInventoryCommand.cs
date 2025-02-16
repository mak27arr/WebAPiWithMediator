using MediatR;

namespace Inventory.Application.Features.Inventory.Commands
{
    public class RemoveProductFromInventoryCommand : IRequest<bool>
    {
        public int ProductId { get; set; }

        public int Quantity { get; set; }
    }
}
