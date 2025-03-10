using Inventory.Application.Features.Inventory.Commands;
using Inventory.Domain.Events;
using Inventory.Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;
using Products.Common.Kafka;
using Products.Common.Kafka.EventArg.Inventory;

namespace Inventory.Application.Features.Inventory.Handlers
{
    public class ReserveInventoryByOrderHandler : IRequestHandler<ReserveInventoryByOrderCommand>
    {
        private readonly IKafkaProducer _kafkaProducer;
        private readonly ILogger<ReserveInventoryByOrderHandler> _logger;
        private readonly IMediator _mediator;

        public ReserveInventoryByOrderHandler(
            IKafkaProducer kafkaProducer,
            IMediator mediator,
            ILogger<ReserveInventoryByOrderHandler> logger)
        {
            _kafkaProducer = kafkaProducer;
            _mediator = mediator;
            _logger = logger;
        }

        public async Task Handle(ReserveInventoryByOrderCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Checking inventory for OrderId: {request.OrderId}");

            var reserveCommand = new ReserveInventoryCommand()
            {
                ReferenceId = request.OrderId.ToString(),
                ReferenceType = EventReferenceType.Order,
                ProductRequest = new ProductStoreModel()
                {
                    ProductId = request.Product.Value,
                    Quantity = request.Quantity
                }
            };

            try
            {
                await _mediator.Send(reserveCommand, cancellationToken);
                await SendReservationSucceeded(request);
            }
            catch(InvalidOperationException ex)
            {
                await SendReservationFailed(request, ex.Message);
            }
        }

        private async Task SendReservationSucceeded(ReserveInventoryByOrderCommand request)
        {
            var reservedEvent = new InventoryReservedEvent
            {
                OrderId = request.OrderId,
            };

            await _kafkaProducer.ProduceAsync(KafkaInventoryTopics.InventoryReservationSucceeded, reservedEvent);
        }

        private async Task SendReservationFailed(ReserveInventoryByOrderCommand request, string message)
        {
            _logger.LogWarning($"Failed reservation for OrderId: {request.OrderId} {message}");

            var notAvailableEvent = new InventoryNotAvailableEvent
            {
                OrderId = request.OrderId,
                Message = message,
            };

            await _kafkaProducer.ProduceAsync(KafkaInventoryTopics.InventoryReservationFailed, notAvailableEvent);
        }
    }
}
