namespace Products.Common.Kafka.EventArg
{
    public class OrderCreatedEvent : BaseEvent
    {
        public override string Topic => KafkaOrderTopics.OrderCreated;

        public Guid OrderId { get; init; }
        public int? ProductId { get; init; }
        public int Quantity { get; init; }
    }
}
