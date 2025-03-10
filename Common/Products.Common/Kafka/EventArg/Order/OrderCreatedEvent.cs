namespace Products.Common.Kafka.EventArg
{
    public class OrderCreatedEvent
    {
        public Guid OrderId;
        public int? ProductId;
        public int Quantity;
    }
}
