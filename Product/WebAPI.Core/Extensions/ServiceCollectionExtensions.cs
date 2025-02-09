using Microsoft.Extensions.DependencyInjection;
using WebAPI.Core.Handlers;

namespace WebAPI.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureMediator(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<MediatorRegister>());
            return services;
        }
    }
}
