﻿using Inventory.Domain.Events;
using MediatR;

namespace Inventory.Application.Features.Inventory.Commands
{
    public class AddProductToInventoryCommand : IRequest<bool>
    {
        public string? ReferenceId { get; set; }

        public EventReferenceType ReferenceType { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }
    }
}
