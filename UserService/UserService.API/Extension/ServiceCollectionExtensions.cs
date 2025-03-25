using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Products.Core.Mapping;
using UserService.API.HttpContext;
using UserService.Application.Interface.Services;

namespace UserService.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services)
        {
            services.AddTransient<IUserContextService, UserContextService>();
            services.AddHttpContextAccessor();
            return services;
        }
    }
}
