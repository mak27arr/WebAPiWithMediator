using Microsoft.Extensions.DependencyInjection;
using Inventory.Infrastructure.Extensions;
using Microsoft.Extensions.Configuration;
using Inventory.Application.Features;
using Inventory.Application.Mappings;
using Inventory.Application.Interface;
using Inventory.Application.Services.Grpc;
using Inventory.Application.Interface.Services;
using Inventory.Application.Services;

namespace Inventory.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddInfrastructureServices(configuration);
            services.RegisterGrpcService(configuration);
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
