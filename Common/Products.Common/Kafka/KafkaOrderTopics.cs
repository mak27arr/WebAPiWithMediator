namespace Products.Common.Kafka
{
    public static class KafkaOrderTopics
    {
        public const string OrderCreated = "order-created";
        public const string OrderUpdated = "order-updated";
        public const string OrderDeleted = "order-deleted";
        public const string OrderPaid = "order-paid";
        public const string OrderFailed = "order-failed";
    }
}
