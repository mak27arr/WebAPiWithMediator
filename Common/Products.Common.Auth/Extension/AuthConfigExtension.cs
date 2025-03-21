using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace Products.Common.Auth.Extension
{
    public static class AuthConfigExtension
    {
        private static readonly string _authorityConfigKey = "AzureAd";
        private static readonly string _authorityClinetIdConfigKey = "ClientId";
        private static readonly string _authorityTenantIdConfigKey = "TenantId";
        private static readonly string _authorityAuthorityConfigKey = "Authority";

        public static IServiceCollection AddAuthConfig(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    var azureAdSection = configuration.GetSection(_authorityConfigKey);
                    var tenantId = azureAdSection[_authorityTenantIdConfigKey] ?? configuration["AZUREAD__TENANTID"];
                    var clientId = azureAdSection[_authorityClinetIdConfigKey] ?? configuration["AZUREAD__CLIENTID"];
                    var authority = azureAdSection[_authorityAuthorityConfigKey] ?? configuration["AZUREAD__AUTHORITY"];
                    options.Authority = authority;
                    options.Audience = clientId;

                    if (environment.IsDevelopment())
                        options.RequireHttpsMetadata = false;

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = authority,
                        ValidateAudience = true,
                        ValidAudience = clientId,
                        ValidateLifetime = true
                    };
                });
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
            return !string.IsNullOrWhiteSpace(configuration[$"{_authorityConfigKey}:{_authorityClinetIdConfigKey}"]);
        }
    }
}
