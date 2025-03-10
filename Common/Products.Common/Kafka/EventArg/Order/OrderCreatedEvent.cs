namespace Products.Common.Kafka.EventArg
{
    public class OrderCreatedEvent
    {
        public Guid OrderId { get; init; }
        public int? ProductId { get; init; }
        public int Quantity { get; init; }
    }
}
