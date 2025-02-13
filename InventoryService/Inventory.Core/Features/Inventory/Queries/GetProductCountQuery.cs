using MediatR;

namespace Inventory.Application.Features.Inventory.Queries
{
    public class GetProductCountQuery : IRequest<int?>
    {
        public int ProductId { get; set; }
    }
}
