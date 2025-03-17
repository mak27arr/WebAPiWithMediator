namespace Products.Common.Kafka.EventArg
{
    public class OrderCreatedEvent : BaseEvent
    {
        public Guid OrderId { get; init; }
        public int? ProductId { get; init; }
        public int Quantity { get; init; }
    }
}
