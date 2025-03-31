using OrderService.Domain.Interface.Repository;
using OrderService.Infrastructure.Persistence;
using OrderService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Products.Common.Kafka;
using Products.Common.Kafka.Producer;

namespace OrderService.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext(configuration);
            services.AddRepositories();
            services.AddSingleton<IKafkaProducer, KafkaProducer>();
            return services;
        }

        public static IServiceScope ApplyInfrastructureMaintenanceJobs(this IServiceScope scope)
        {
            using (var dbContext = scope.ServiceProvider.GetRequiredService<OrderServiceDbContext>())
                dbContext.Database.Migrate();

            return scope;
        }

        internal static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<OrderServiceDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
            return services;
        }

        internal static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IOrderRepository, OrderRepository>();
            return services;
        }
    }
}
