namespace Products.Common.Kafka.EventArg.Inventory
{
    public class InventoryNotAvailableEvent : BaseEvent
    {
        public required Guid OrderId { get; init; }

        public required string Message { get; init; }

        public override string Topic => KafkaInventoryTopics.InventoryReservationFailed;
    }
}
