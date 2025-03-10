using Microsoft.Extensions.DependencyInjection;
using OrderService.Infrastructure.Extensions;
using Microsoft.Extensions.Configuration;
using OrderService.Application.Features;

namespace OrderService.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddInfrastructureServices(configuration);
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<MediatorRegister>());
            return services;
        }
    }
}
