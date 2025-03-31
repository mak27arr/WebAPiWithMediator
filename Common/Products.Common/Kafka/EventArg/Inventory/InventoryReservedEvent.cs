namespace Products.Common.Kafka.EventArg.Inventory
{
    public class InventoryReservedEvent : BaseEvent
    {
        public Guid OrderId { get; init; }

        public override string Topic => KafkaInventoryTopics.InventoryReservationSucceeded;
    }
}
