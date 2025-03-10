using MediatR;

namespace OrderService.Application.Features.Commands
{
    public record CreateOrderCommand(int ProductId, int Quantity, Guid UserId) : IRequest<Guid>;
}
