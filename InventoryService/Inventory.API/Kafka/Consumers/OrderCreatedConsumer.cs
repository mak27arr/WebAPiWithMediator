using Confluent.Kafka;
using Inventory.Application.Features.Inventory.Commands;
using MediatR;
using Newtonsoft.Json;
using Products.Common.Kafka.EventArg;

namespace Inventory.API.Kafka.Consumers
{
    public class OrderCreatedConsumer : ConsumerBase<OrderCreatedEvent>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public OrderCreatedConsumer(IServiceScopeFactory serviceScopeFactory,
            IConfiguration config,
            ILogger<OrderCreatedConsumer> logger) : base(config, logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ProcessMessage(ConsumeResult<Ignore, string> consumeResult, CancellationToken stoppingToken)
        {
            var message = consumeResult.Message.Value;
            var order = JsonConvert.DeserializeObject<OrderCreatedEvent>(message);

            using var scope = _serviceScopeFactory.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            await mediator.Send(new ReserveInventoryByOrderCommand(order), stoppingToken);
        }
    }
}
