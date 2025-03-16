using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Builder;

namespace Products.Common.Auth.Extension
{
    public static class AuthConfigExtension
    {
        private static readonly string _authorityConfigKey = "AzureAdB2C";

        public static IServiceCollection AddAuthConfig(this IServiceCollection services, IConfiguration configuration)
        {
            if (!IsAuthAvailable(configuration))
                return services;

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApi(configuration.GetSection(_authorityConfigKey));
            services.AddAuthorization();

            return services;
        }

        public static IApplicationBuilder ConfigureAuthentication(this IApplicationBuilder app, IConfiguration configuration)
        {
            if (IsAuthAvailable(configuration))
            {
                app.UseAuthentication();
                app.UseAuthorization();
            }

            return app;
        }

        private static bool IsAuthAvailable(IConfiguration configuration)
        {
            return !string.IsNullOrWhiteSpace(configuration[_authorityConfigKey]);
        }
    }
}
