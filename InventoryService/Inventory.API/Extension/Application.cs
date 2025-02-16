using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Inventory.API.Extension
{
    internal static class Application
    {
        internal static IServiceCollection AddCustomCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAnyOrigin", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });

            return services;
        }

        internal static IServiceCollection AddProductHealthChecks(this IServiceCollection services)
        {
            services.AddHealthChecks()
                    .AddCheck("Product Service", () => HealthCheckResult.Healthy("Product Service is running."));

            return services;
        }
    }
}
