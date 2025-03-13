using Confluent.Kafka;
using Inventory.Application.Features.Inventory.Commands;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Products.Common.Kafka;
using Products.Common.Kafka.EventArg;

namespace Inventory.API.Kafka.Consumers
{
    public class OrderCreatedConsumer : BackgroundService
    {
        private readonly IConfiguration _config;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<OrderCreatedConsumer> _logger;

        public OrderCreatedConsumer(IServiceScopeFactory serviceScopeFactory,
            IConfiguration config,
            ILogger<OrderCreatedConsumer> logger)
        {
            _config = config;
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("InventoryService Kafka Consumer started. Listening for new orders...");
            await StartConsumeTask(stoppingToken);
        }

        private async Task StartConsumeTask(CancellationToken stoppingToken)
        {
            var config = GetConfig(_config);

            using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
            consumer.Subscribe(KafkaOrderTopics.OrderCreated);

            var consumerTask = Task.Factory.StartNew(async () =>
            {
                try
                {
                    while (!stoppingToken.IsCancellationRequested)
                    {
                        var result = consumer.Consume(stoppingToken);
                        _logger.LogInformation($"Received order event: {result}");
                        await ProccesOrderCreatedMessage(result, stoppingToken);
                        consumer.Commit(result);
                    }
                }
                catch (ConsumeException ex)
                {
                    _logger.LogError($"Error consuming Kafka message: {ex.Message}");
                }
                catch (OperationCanceledException)
                {
                    _logger.LogError("Consumer stopped.");
                }
            }, stoppingToken, TaskCreationOptions.LongRunning, TaskScheduler.Default);

            await consumerTask;
            consumer?.Close();
        }

        private async Task ProccesOrderCreatedMessage(ConsumeResult<Ignore, string> consumer, CancellationToken stoppingToken)
        {
            var message = consumer.Message.Value;
            var order = JsonConvert.DeserializeObject<OrderCreatedEvent>(message);
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                await mediator.Send(new ReserveInventoryByOrderCommand(order), stoppingToken);
            }
        }

        private IEnumerable<KeyValuePair<string, string>> GetConfig(IConfiguration configuration)
        {
            var kafkaHost = configuration.GetSection("Kafka:Host").Value;
            var kafkaPort = configuration.GetSection("Kafka:Port").Value;
            var GroupId = configuration.GetSection("Kafka:GroupId").Value;

            return new ConsumerConfig
            {
                BootstrapServers = $"{kafkaHost}:{kafkaPort}",
                GroupId = GroupId,
                AutoOffsetReset = AutoOffsetReset.Latest,
                EnableAutoCommit = false,
            };
        }
    }
}
