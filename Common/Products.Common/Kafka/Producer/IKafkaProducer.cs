using Products.Common.Kafka.EventArg;

namespace Products.Common.Kafka
{
    public interface IKafkaProducer
    {
        Task ProduceAsync<T>(string topic, T message);

        Task ProduceAsync<T>(T message) where T : BaseEvent;
    }
}
