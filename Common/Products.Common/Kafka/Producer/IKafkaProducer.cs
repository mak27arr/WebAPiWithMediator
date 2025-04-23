using Products.Common.Kafka.EventArg;

namespace Products.Common.Kafka
{
    public interface IKafkaProducer
    {
        Task ProduceAsync<T>(string topic, T message, CancellationToken token = default);

        Task ProduceAsync<T>(T message, CancellationToken token = default) where T : BaseEvent;
    }
}
