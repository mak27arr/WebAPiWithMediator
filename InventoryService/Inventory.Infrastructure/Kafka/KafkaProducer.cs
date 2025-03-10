using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Products.Common.Kafka;
using System.Text.Json;

namespace Inventory.Infrastructure.Kafka
{
    internal class KafkaProducer : IKafkaProducer
    {
        private readonly IProducer<Null, string> _producer;

        public KafkaProducer(IConfiguration configuration)
        {
            var config = GetConfig(configuration);
            _producer = new ProducerBuilder<Null, string>(config).Build();
        }

        private ProducerConfig GetConfig(IConfiguration configuration)
        {
            var kafkaHost = configuration.GetSection("Kafka:Host").Value;
            var kafkaPort = configuration.GetSection("Kafka:Port").Value;

            return new ProducerConfig { BootstrapServers = $"{kafkaHost}:{kafkaPort}" };
        }

        public async Task ProduceAsync<T>(string topic, T message)
        {
            var json = JsonSerializer.Serialize(message);
            await _producer.ProduceAsync(topic, new Message<Null, string> { Value = json });
        }
    }
}
