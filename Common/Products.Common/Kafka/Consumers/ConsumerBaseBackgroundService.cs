using Microsoft.Extensions.Hosting;
using Products.Common.Kafka.EventArg;

namespace Inventory.API.Kafka.Consumers
{
    public abstract class ConsumerBaseBackgroundService<TEvent> : BackgroundService where TEvent : BaseEvent, new()
    {
        private readonly ConsumerBase<TEvent> _consumerBase;

        protected ConsumerBaseBackgroundService(ConsumerBase<TEvent> consumerBase)
        {
            _consumerBase = consumerBase;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _consumerBase.StartConsumeTask(stoppingToken);
        }
    }
}
