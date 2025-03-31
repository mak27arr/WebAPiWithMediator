using Inventory.API.Kafka.Consumers;

namespace Inventory.API.Extension
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCommunicationServices(this IServiceCollection services)
        {
            services.AddSingleton<OrderCreatedConsumer>();
            services.AddHostedService<OrderCreatedConsumerBackgroundService>();
            return services;
        }
    }
}
