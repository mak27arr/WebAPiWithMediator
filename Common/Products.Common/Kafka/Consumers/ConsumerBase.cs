using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Products.Common.Kafka.EventArg;

namespace Inventory.API.Kafka.Consumers
{
    public abstract class ConsumerBase<TEvent> where TEvent : BaseEvent, new()
    {
        private readonly IConfiguration _config;
        private readonly ILogger _logger;
        private readonly string _topic;

        protected ConsumerBase(
            IConfiguration config,
            ILogger logger)
        {
            _config = config;
            _logger = logger;
            _topic = GetTopicName();
        }

        private string GetTopicName() => new TEvent().Topic;

        public async Task StartConsumeTask(CancellationToken stoppingToken)
        {
            var config = GetKafkaConfig();

            using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
            consumer.Subscribe(_topic);

            var consumerTask = Task.Factory.StartNew(async () =>
            {
                try
                {
                    while (!stoppingToken.IsCancellationRequested)
                    {
                        var result = consumer.Consume(stoppingToken);
                        _logger.LogInformation("Received event from topic '{0}': {1}", _topic, result.Message.Value);
                        await ProcessMessage(result, stoppingToken);
                        consumer.Commit(result);
                    }
                }
                catch (ConsumeException ex)
                {
                    _logger.LogError("Kafka consume error: {0}", ex.Message);
                }
                catch (OperationCanceledException)
                {
                    _logger.LogInformation("Kafka Consumer for topic '{0}' stopped.", _topic);
                }
            }, stoppingToken, TaskCreationOptions.LongRunning, TaskScheduler.Default);

            await consumerTask;
            consumer.Close();
        }

        private ConsumerConfig GetKafkaConfig()
        {
            var kafkaHost = _config.GetSection("Kafka:Host").Value;
            var kafkaPort = _config.GetSection("Kafka:Port").Value;
            var groupId = _config.GetSection("Kafka:GroupId").Value;

            return new ConsumerConfig
            {
                BootstrapServers = $"{kafkaHost}:{kafkaPort}",
                GroupId = groupId,
                AutoOffsetReset = AutoOffsetReset.Latest,
                EnableAutoCommit = false,
            };
        }

        protected abstract Task ProcessMessage(ConsumeResult<Ignore, string> consumeResult, CancellationToken stoppingToken);
    }
}
