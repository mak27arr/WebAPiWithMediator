using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using OpenTelemetry.Trace;
using OpenTelemetry.Metrics;
using Confluent.Kafka.Extensions.OpenTelemetry;
using OpenTelemetry.Resources;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

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

        public static IServiceCollection AddApplicationOpenTelemetry(this IServiceCollection services, IHostEnvironment envirovment)
        {
            var name = envirovment.ApplicationName;
            services.AddOpenTelemetry()
                .WithMetrics(builder =>
                {
                    builder
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddEventCountersInstrumentation(options =>
                    {
                        options.AddEventSources("Microsoft.AspNetCore.Hosting", "Microsoft-AspNetCore-Server-Kestrel");
                    })
                    .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(name))
                    .AddPrometheusExporter();
                });

            //services.AddElasticApm(Configuration);
            services.AddOpenTelemetry().WithTracing(builder =>
            {
                builder
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddGrpcClientInstrumentation()
                .AddConfluentKafkaInstrumentation()
                .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(name));
            });

            return services;
        }
    }
}
