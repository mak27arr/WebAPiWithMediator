using Inventory.Domain.Interface.Repository;
using Inventory.Domain.Repositories;
using Inventory.Infrastructure.Mappings;
using Inventory.Infrastructure.Persistence;
using Inventory.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Products.Common.Kafka;
using Products.Common.Kafka.Producer;
using Products.Common.Protos.Product;

namespace Inventory.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext(configuration);
            services.AddRepositories();
            services.AddSingleton<IKafkaProducer, KafkaProducer>();
            services.RegisterGrpcService(configuration);
            services.AddAutoMapper(typeof(MappingProfile));
            return services;
        }

        public static IServiceScope ApplyInfrastructureMaintenanceJobs(this IServiceScope scope)
        {
            using (var dbContext = scope.ServiceProvider.GetRequiredService<InventoryDbContext>())
                dbContext.Database.Migrate();

            return scope;
        }

        internal static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<InventoryDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
            return services;
        }

        internal static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IEventStoreRepository, EventStoreRepository>();
            services.AddScoped<IInventoryRepository, InventoryRepository>();
            services.AddScoped<IInventoryUow, InventoryUow>();
            return services;
        }

        private static IServiceCollection RegisterGrpcService(this IServiceCollection services, IConfiguration configuration)
        {
            var grpcUrl = configuration.GetValue<string>("GrpcSettings:ProductServiceUrl");

            services.AddGrpcClient<ProductService.Grpc.ProductService.ProductServiceClient>(options =>
            {
                options.Address = new Uri(grpcUrl);
            });
            services.AddScoped<IProductGrpcService, ProductGrpcService>();

            return services;
        }
    }
}
