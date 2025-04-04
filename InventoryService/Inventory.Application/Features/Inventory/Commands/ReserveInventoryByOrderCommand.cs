using FluentResults;
using MediatR;
using Products.Common.Kafka.EventArg;

namespace Inventory.Application.Features.Inventory.Commands
{
    public class ReserveInventoryByOrderCommand : IRequest
    {
        public Guid OrderId { get; }
        public int? Product { get; }
        public int Quantity { get; }

        public ReserveInventoryByOrderCommand(OrderCreatedEvent order)
        {
            OrderId = order.OrderId;
            Product = order.ProductId;
            Quantity = order.Quantity;
        }
    }
}
