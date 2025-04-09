using Confluent.Kafka;
using Inventory.Application.Features.Inventory.Commands;
using MediatR;
using Newtonsoft.Json;
using Products.Common.Kafka;
using Products.Common.Kafka.EventArg;
using Products.Common.Kafka.EventArg.Inventory;

namespace Inventory.API.Kafka.Consumers
{
    public class OrderCreatedConsumer : ConsumerBase<OrderCreatedEvent>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IKafkaProducer _kafkaProducer;
        private readonly ILogger<OrderCreatedConsumer> _logger;

        public OrderCreatedConsumer(IServiceScopeFactory serviceScopeFactory,
            IConfiguration config,
            IKafkaProducer kafkaProducer,
            ILogger<OrderCreatedConsumer> logger) : base(config, logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _kafkaProducer = kafkaProducer;
            _logger = logger;
        }

        protected override async Task ProcessMessage(ConsumeResult<Ignore, string> consumeResult, CancellationToken stoppingToken)
        {
            var message = consumeResult.Message.Value;
            var order = JsonConvert.DeserializeObject<OrderCreatedEvent>(message);

            using var scope = _serviceScopeFactory.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            try
            {
                if (order != null)
                {
                    await mediator.Send(new ReserveInventoryByOrderCommand(order), stoppingToken);
                    await SendReservationSucceeded(order);
                }
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Inventory reservation failed for product {0}: {1}", order?.ProductId, ex.Message);
                await SendReservationFailed(order, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while reserving inventory for product {0}", order?.ProductId);
                throw;
            }
        }

        private async Task SendReservationSucceeded(OrderCreatedEvent order)
        {
            var reservedEvent = new InventoryReservedEvent
            {
                OrderId = order.OrderId
            };

            await _kafkaProducer.ProduceAsync(reservedEvent);
        }

        private async Task SendReservationFailed(OrderCreatedEvent? order, string message)
        {
            var notAvailableEvent = new InventoryNotAvailableEvent
            {
                OrderId = order?.OrderId ?? Guid.Empty,
                Message = message
            };

            await _kafkaProducer.ProduceAsync(notAvailableEvent);
        }
    }
}
