using Microsoft.Extensions.DependencyInjection;
using Inventory.Infrastructure.Extensions;
using Microsoft.Extensions.Configuration;
using Inventory.Application.Features;
using Inventory.Application.Mappings;

namespace Inventory.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddInfrastructureServices(configuration);
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<MediatorRegister>());
            services.AddAutoMapper(typeof(MappingProfile));
            return services;
        }

        public static IServiceCollection ApplyApplicationMaintenanceJobs(this IServiceCollection services)
        {
            using (var scope = services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
                dbContext.Database.Migrate();
            }

            return services;
        }
    }
}
