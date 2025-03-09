namespace Products.Common.Kafka
{
    public interface IKafkaProducer
    {
        Task ProduceAsync<T>(string topic, T message);
    }
}
