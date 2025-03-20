using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Web;

namespace Products.Common.Auth.Extension
{
    public static class AuthConfigExtension
    {
        private static readonly string _authorityConfigKey = "AzureAd:ClientId";

        public static IServiceCollection AddAuthConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApi(configuration, _authorityConfigKey);
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
