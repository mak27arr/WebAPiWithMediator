using JwtAuthManager.Handler;
using JwtAuthManager.Interface;
using Microsoft.Extensions.DependencyInjection;

namespace JwtAuthManager.Extension
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureMediator(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<MediatorRegister>());
            return services;
        }

        public static IServiceCollection AddJwtAuthManager(this IServiceCollection services)
        {
            services.AddScoped<ITokenService, TokenService>();
            return services;
        }
    }
}
