using Inventory.Domain.Events;
using Inventory.Domain.ValueObjects;
using MediatR;

namespace Inventory.Application.Features.Inventory.Commands
{
    public class ReserveInventoryCommand : IRequest
    {
        public string? ReferenceId { get; set; }

        public EventReferenceType ReferenceType { get; set; }

        public ProductStoreModel ProductRequest { get; init; }
    }
}
