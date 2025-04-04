using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using Products.Common.Kafka.EventArg;
using Serilog.Context;
using System.Diagnostics;
using System.Text;

namespace Inventory.API.Kafka.Consumers
{
    public abstract class ConsumerBase<TEvent> where TEvent : BaseEvent, new()
    {
        private readonly IConfiguration _config;
        private readonly ILogger _logger;
        private readonly string _topic;
        private readonly AsyncRetryPolicy _retryPolicy;
        private static readonly ActivitySource _activitySource = new("KafkaConsumer");

        protected ConsumerBase(
            IConfiguration config,
            ILogger logger)
        {
            _config = config;
            _logger = logger;
            _topic = GetTopicName();

            _retryPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
            (exception, timeSpan, retryCount, context) =>
            {
                _logger.LogWarning($"Attempt {retryCount} failed. Retrying in {timeSpan.TotalSeconds} seconds...");
            });
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

                    using (var activity = _activitySource.StartActivity("Kafka ProcessMessage", ActivityKind.Consumer))
                    using (LogContext.PushProperty("X-Session-Id", sessionId))
                    {
                        SetActivityParams(result, sessionId, activity);
                        _logger.LogInformation("Received event from topic '{0}': {1}", _topic, result.Message.Value);

                        var isSuccess = await _retryPolicy.ExecuteAsync(async () =>
                        {
                            await ProcessMessage(result, stoppingToken);
                            return true;
                        });

                        consumer.Commit(result);

                        if (!isSuccess)
                        {
                            _logger.LogError("Message processing failed after. Sending to Dead Letter Queue...");
                            await SendToDeadLetterQueue(result);
                        }

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

        private void SetActivityParams(ConsumeResult<Ignore, string> result, string sessionId, Activity? activity)
        {
            if (activity != null)
            {
                if (!string.IsNullOrEmpty(sessionId))
                    activity.SetParentId(sessionId);

                activity.SetTag("kafka.topic", _topic);
                activity.SetTag("kafka.partition", result.Partition.Value);
                activity.SetTag("kafka.offset", result.Offset.Value);
                activity.SetTag("message.key", result.Message.Key);
                activity.SetTag("message.value", result.Message.Value);
            }
        }

        protected virtual async Task SendToDeadLetterQueue(ConsumeResult<Ignore, string> result)
        {
            //TODO
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
