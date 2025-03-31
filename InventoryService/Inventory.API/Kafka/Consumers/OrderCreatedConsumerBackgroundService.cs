using Products.Common.Kafka.EventArg;

namespace Inventory.API.Kafka.Consumers
{
    public class OrderCreatedConsumerBackgroundService : ConsumerBaseBackgroundService<OrderCreatedEvent>
    {
        public OrderCreatedConsumerBackgroundService(OrderCreatedConsumer consumerBase) : base(consumerBase)
        {
        }
    }
}
