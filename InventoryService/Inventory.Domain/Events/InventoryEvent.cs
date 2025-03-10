namespace Inventory.Domain.Events
{
    public class InventoryEvent
    {
        public int Id { get; set; }

        public int? ProductId { get; set; }

        public int? Quantity { get; set; }

        public string? Action { get; set; }

        public string? ReferenceId { get; set; }

        public EventReferenceType ReferenceType { get; set; }

        public DateTime? Timestamp { get; set; }
    }
}
