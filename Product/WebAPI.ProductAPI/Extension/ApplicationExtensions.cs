using Asp.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using WebAPI.Infrastructure.Data;

namespace WebAPI.ProductAPI.Extension
{
    internal static class Application
    {
        internal static void MigrateDatabase(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
                dbContext.Database.Migrate();
            }
        }

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

        internal static IServiceCollection ConfigureApiVersion(this IServiceCollection services)
        {
            var apiVersioningBuilder = services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
            });

            apiVersioningBuilder.AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
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
