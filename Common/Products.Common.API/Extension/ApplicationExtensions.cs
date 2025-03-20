using Asp.Versioning;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Products.Common.API.Extension
{
    public static class ApplicationExtensions
    {
        public static IServiceCollection AddCustomCors(this IServiceCollection services)
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

        public static IServiceCollection AddProductHealthChecks(this IServiceCollection services, Func<HealthCheckResult> check = null)
        {
            if (check == null)
                check = () => HealthCheckResult.Healthy("Product Service is running.");

            services.AddHealthChecks().AddCheck("Product Service", check);

            return services;
        }
    }
}
