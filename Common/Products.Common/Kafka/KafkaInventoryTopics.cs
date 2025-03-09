namespace Products.Common.Kafka
{
    public static class KafkaInventoryTopics
    {
        public const string ProductAddedToInventory = "product-added-to-inventory";
        public const string ProductRemovedFromInventory = "product-removed-from-inventory";
        public const string InventoryChecked = "inventory-checked";
        public const string InventoryReservationSucceeded = "inventory-reservation-succeeded";
        public const string InventoryReservationFailed = "inventory-reservation-failed";
    }
}
