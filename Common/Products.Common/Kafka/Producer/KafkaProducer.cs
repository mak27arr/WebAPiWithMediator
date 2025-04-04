using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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

        public KafkaProducer(IConfiguration configuration, ILogger<KafkaProducer> logger)
        {
            var config = GetConfig(configuration);
            _producer = new ProducerBuilder<Null, string>(config).Build();
            _logger = logger;
        }

        private ProducerConfig GetConfig(IConfiguration configuration)
        {
            var kafkaHost = configuration.GetSection("Kafka:Host").Value;
            var kafkaPort = configuration.GetSection("Kafka:Port").Value;

            return new ProducerConfig { BootstrapServers = $"{kafkaHost}:{kafkaPort}" };
        }

        public async Task ProduceAsync<T>(string topic, T message)
        {
            var jsonMessage = JsonSerializer.Serialize(message);
            var headers = new Headers
            {
                { "X-Session-Id", Encoding.UTF8.GetBytes(Activity.Current?.TraceId.ToString() ?? Guid.NewGuid().ToString()) }
            };
            var kafkaMessage = new Message<Null, string> { Value = jsonMessage, Headers = headers };
            _logger.LogInformation("Send event: {0}", jsonMessage);
            await _producer.ProduceAsync(topic, kafkaMessage);
        }

        public async Task ProduceAsync<T>(T message) where T : BaseEvent
        {
            await ProduceAsync(message.Topic, message);
        }
    }
}
