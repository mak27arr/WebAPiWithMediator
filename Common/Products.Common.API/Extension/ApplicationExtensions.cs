using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using OpenTelemetry.Trace;
using OpenTelemetry.Metrics;
using Confluent.Kafka.Extensions.OpenTelemetry;
using OpenTelemetry.Resources;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Configuration;

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

        public static IServiceCollection AddProductHealthChecks(this IServiceCollection services,
            Func<HealthCheckResult>? serviceCheck = null,
            Action<IHealthChecksBuilder>? configure = null)
        {
            serviceCheck ??= () => HealthCheckResult.Healthy("Service is running.");

            var builder = services.AddHealthChecks()
                .AddCheck("Service", check: serviceCheck);

            configure?.Invoke(builder);

            return services;
        }

        public static IServiceCollection AddApplicationOpenTelemetry(this IServiceCollection services, IHostEnvironment envirovment)
        {
            var name = envirovment.ApplicationName;
            //services.AddElasticApm(Configuration);
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
                }).WithTracing(builder =>
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
