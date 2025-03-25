using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Products.Core.Mapping;
using UserService.Application.Feature;
using UserService.Application.Interface.Services;
using UserService.Application.Services;
using UserService.Infrastructure.Extensions;

namespace UserService.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddInfrastructureServices(configuration);
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<MediatorRegister>());
            services.AddMapperProfile();
            services.AddTransient<IUserProfileService, UserProfileService>();
            return services;
        }

        public static IServiceCollection AddMapperProfile(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfile).Assembly);
            return services;
        }
    }
}
