using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Products.Common.API.Extension
{
    public static class SwaggerServiceExtensions
    {
        public static void AddSwaggerServices(this IServiceCollection services, string title = "My API", params string[] versions)
        {
            services.AddSwaggerGen(c =>
            {
                foreach (var version in versions)
                    c.SwaggerDoc(version, new OpenApiInfo { Title = title, Version = version });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter JWT token in the format: Bearer {your-token}"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                        },
                        new string[] { }
                    }
                });
            });
        }

        public static void UseCustomSwagger(this IApplicationBuilder app, string title = "My API", params string[] versions)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                foreach (var version in versions)
                    c.SwaggerEndpoint($"/swagger/{version}/swagger.json", title);

                c.RoutePrefix = string.Empty;
            });
        }
    }
}
