namespace Products.Common.Kafka.EventArg.Inventory
{
    public class InventoryNotAvailableEvent
    {
        public Guid OrderId { get; init; }

        public string Message { get; init; }
    }
}
