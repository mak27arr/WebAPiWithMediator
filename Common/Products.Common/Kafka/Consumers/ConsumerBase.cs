using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Products.Common.Kafka.EventArg;
using Serilog.Context;
using System.Text;

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
            _logger.LogInformation("Start consume {0}", GetTopicName());
            var config = GetKafkaConfig();
            using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();

            await Task.Run(() => ProcessConsumeMessage(consumer, stoppingToken), stoppingToken);

            consumer.Close();
            _logger.LogInformation("Stop consume {0}", GetTopicName());
        }

        private async Task ProcessConsumeMessage(IConsumer<Ignore, string> consumer, CancellationToken stoppingToken)
        {
            try
            {
                consumer.Subscribe(_topic);

                while (!stoppingToken.IsCancellationRequested)
                {
                    var result = consumer.Consume(stoppingToken);
                    var sessionId = GetSesionId(result);

                    using (LogContext.PushProperty("X-Session-Id", sessionId))
                    {
                        _logger.LogInformation("Received event from topic '{0}': {1}", _topic, result.Message.Value);
                        await ProcessMessage(result, stoppingToken);
                        consumer.Commit(result);
                    }
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
        }

        private static string GetSesionId(ConsumeResult<Ignore, string> result)
        {
            return result.Message.Headers.TryGetLastBytes("X-Session-Id", out var sessionIdBytes)
                ? Encoding.UTF8.GetString(sessionIdBytes)
                : "UnknownSessionId";
        }

        protected virtual ConsumerConfig GetKafkaConfig()
        {
            GetKafkaHostConfig(out var kafkaHost, out var kafkaPort, out var groupId);

            return new ConsumerConfig
            {
                BootstrapServers = $"{kafkaHost}:{kafkaPort}",
                GroupId = groupId,
                AutoOffsetReset = AutoOffsetReset.Latest,
                EnableAutoCommit = false,
            };
        }

        private void GetKafkaHostConfig(out string? kafkaHost, out string? kafkaPort, out string? groupId)
        {
            kafkaHost = _config.GetSection("Kafka:Host").Value;
            kafkaPort = _config.GetSection("Kafka:Port").Value;
            groupId = _config.GetSection("Kafka:GroupId").Value;
        }

        protected abstract Task ProcessMessage(ConsumeResult<Ignore, string> consumeResult, CancellationToken stoppingToken);
    }
}
