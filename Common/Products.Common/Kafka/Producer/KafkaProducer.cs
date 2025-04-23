using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Polly;
using Products.Common.Kafka.EventArg;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace Products.Common.Kafka.Producer
{
    public class KafkaProducer : IKafkaProducer
    {
        private readonly IProducer<Null, string> _producer;
        private readonly ILogger<KafkaProducer> _logger;
        private readonly IAsyncPolicy _retryPolicy;

        public KafkaProducer(IConfiguration configuration, 
            ILogger<KafkaProducer> logger, 
            IRetryPolicyProvider retryPolicyProvider)
        {
            var config = GetConfig(configuration);
            _producer = new ProducerBuilder<Null, string>(config).Build();
            _logger = logger;
            _retryPolicy = retryPolicyProvider.GetKafkaProducerPolicy();
        }

        private ProducerConfig GetConfig(IConfiguration configuration)
        {
            var kafkaHost = configuration.GetSection("Kafka:Host").Value;
            var kafkaPort = configuration.GetSection("Kafka:Port").Value;

            return new ProducerConfig { BootstrapServers = $"{kafkaHost}:{kafkaPort}" };
        }

        public async Task ProduceAsync<T>(string topic, T message, CancellationToken token = default)
        {
            var jsonMessage = JsonSerializer.Serialize(message);
            var headers = new Headers
            {
                { "X-Session-Id", Encoding.UTF8.GetBytes(Activity.Current?.TraceId.ToString() ?? Guid.NewGuid().ToString()) }
            };
            var kafkaMessage = new Message<Null, string> { Value = jsonMessage, Headers = headers };

            try
            {
                _logger.LogInformation("Send Kafka event to {Topic}: {Message}", topic, jsonMessage);
                await _retryPolicy.ExecuteAsync(() => _producer.ProduceAsync(topic, kafkaMessage, token));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Failed to send {Topic} after multiple retries. Message: {kafkaMessage}", topic, kafkaMessage);
            }
        }

        public async Task ProduceAsync<T>(T message, CancellationToken token = default) where T : BaseEvent
        {
            await ProduceAsync(message.Topic, message, token);
        }
    }
}
