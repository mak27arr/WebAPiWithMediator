using Microsoft.Extensions.DependencyInjection;
using Products.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Products.Infrastructure.Caching;
using Products.Infrastructure.Interfaces.Repository;
using Products.Infrastructure.Interfaces.Caching;

namespace Products.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDataContextServices(configuration);
            services.AddRepositories();
            services.AddCacheServices();
            return services;
        }

        private static IServiceCollection AddCacheServices(this IServiceCollection services)
        {
            services.AddSingleton<RedisCacheConfiguration>();
            services.AddSingleton(sp =>
            {
                var redisConfig = sp.GetRequiredService<RedisCacheConfiguration>();
                return redisConfig.GetConnection();
            });

            services.AddSingleton(typeof(ICacheService<>), typeof(GenericCacheService<>));

            return services;
        }

        private static IServiceCollection AddDataContextServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DataContext>(options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICurrencyRepository, CurrencyRepository>();
            services.AddScoped<IProductPriceHistoryRepository, ProductPriceHistoryRepository>();
            return services;
        }
    }
}
