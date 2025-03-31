using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Inventory.API.Kafka.Consumers
{
    public abstract class ConsumerBaseBackgroundService<TEvent> : BackgroundService
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
