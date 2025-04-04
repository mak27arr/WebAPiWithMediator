using Microsoft.Extensions.DependencyInjection;
using Inventory.Infrastructure.Extensions;
using Microsoft.Extensions.Configuration;
using Inventory.Application.Features;
using Inventory.Application.Mappings;
using Inventory.Application.Interface.Services;
using Inventory.Application.Services;

namespace Inventory.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddInfrastructureServices(configuration);
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<MediatorRegister>());
            services.AddDomainServices();
            services.AddAutoMapper(typeof(MappingProfile));
            return services;
        }

        private static IServiceCollection AddDomainServices(this IServiceCollection services)
        {
            services.AddScoped<IInventoryService, InventoryService>();
            services.AddScoped<IInventoryApplicationService, InventoryApplicationService>();
            return services;
        }
    }
}
