using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Products.Core.Handlers;
using Products.Core.Mapping;
using Products.Infrastructure.Extensions;

namespace Products.Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureMediator();
            services.AddMapperProfile();
            services.AddInfrastructureServices(configuration);
            return services;
        }

        public static IServiceCollection ConfigureMediator(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<MediatorRegister>());
            return services;
        }

        public static IServiceCollection AddMapperProfile(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfile).Assembly);
            return services;
        }
    }
}
