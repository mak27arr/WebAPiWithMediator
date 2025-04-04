using Asp.Versioning;
using Elastic.CommonSchema;
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
                check = () => HealthCheckResult.Healthy("Service is running.");

            services.AddHealthChecks().AddCheck("Service", check);

            return services;
        }

        public static IServiceCollection AddOpenTelemetry(this IServiceCollection services)
        {
            services.AddOpenTelemetryTracing(builder =>
            {
                builder
                    .AddHttpClientInstrumentation()
                    .AddAspNetCoreInstrumentation()
                    .AddKafkaInstrumentation()
                    .AddConsoleExporter();  // Optional: Export to the console for debugging
            });
        }
    }
}
