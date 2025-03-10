using Confluent.Kafka;
using Inventory.Application.Features.Inventory.Commands;
using MediatR;
using Newtonsoft.Json;
using Products.Common.Kafka;
using Products.Common.Kafka.EventArg;

namespace Inventory.API.Kafka.Consumers
{
    public class OrderCreatedConsumer : BackgroundService
    {
        private readonly IMediator _mediator;
        private readonly IConfiguration _config;
        private readonly ILogger<OrderCreatedConsumer> _logger;

        public OrderCreatedConsumer(IMediator mediator, IConfiguration config,ILogger<OrderCreatedConsumer> logger)
        {
            _mediator = mediator;
            _config = config;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var config = GetConfig(_config);

            using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
            consumer.Subscribe(KafkaOrderTopics.OrderCreated);

            _logger.LogInformation("InventoryService Kafka Consumer started. Listening for new orders...");

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        var consumeResult = consumer.Consume(stoppingToken);
                        var message = consumeResult.Message.Value;

                        _logger.LogInformation($"Received order event: {message}");

                        var order = JsonConvert.DeserializeObject<OrderCreatedEvent>(message);

                        await _mediator.Send(new ReserveInventoryByOrderCommand(order), stoppingToken);
                    }
                    catch (ConsumeException ex)
                    {
                        _logger.LogError($"Error consuming Kafka message: {ex.Message}");
                    }
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Kafka consumer stopped.");
            }
            finally
            {
                consumer?.Close();
            }
        }

        private IEnumerable<KeyValuePair<string, string>> GetConfig(IConfiguration configuration)
        {
            var kafkaHost = configuration.GetSection("Kafka:Host").Value;
            var kafkaPort = configuration.GetSection("Kafka:Port").Value;

            return new ConsumerConfig
            {
                BootstrapServers = $"{kafkaHost}:{kafkaPort}",
                AutoOffsetReset = AutoOffsetReset.Latest
            };
        }
    }
}
