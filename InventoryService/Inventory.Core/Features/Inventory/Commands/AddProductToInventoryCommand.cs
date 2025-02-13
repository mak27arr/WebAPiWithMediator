using MediatR;

namespace Inventory.Application.Features.Inventory.Commands
{
    public class AddProductToInventoryCommand : IRequest<bool>
    {
        public int ProductId { get; set; }

        public int Quantity { get; set; }
    }
}
