namespace Products.Common.Kafka.EventArg.Inventory
{
    public class InventoryNotAvailableEvent : BaseEvent
    {
        public Guid OrderId { get; init; }

        public string Message { get; init; }

        public override string Topic => KafkaInventoryTopics.InventoryReservationFailed;
    }
}
