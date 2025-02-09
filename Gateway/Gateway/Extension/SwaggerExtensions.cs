using Microsoft.OpenApi.Models;

namespace Gateway.Extension
{
    internal static class SwaggerExtensions
    {
        internal static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Ocelot API Gateway",
                    Version = "v1",
                    Description = "API Gateway for Microservices using Ocelot"
                });
                options.SwaggerDoc("webapi_product", new OpenApiInfo
                {
                    Title = "Service 1 API",
                    Version = "v1",
                    Description = "Swagger documentation for Service 1"
                });
            });
        }

        internal static void UseCustomSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API Gateway V1");
                c.SwaggerEndpoint("/webapi_product/swagger/v1/swagger.json", "Service 1 API");
                c.RoutePrefix = "docs";
            });
        }
    }
}
