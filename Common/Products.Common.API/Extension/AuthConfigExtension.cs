using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Products.Common.API.Extension
{
    public static class AuthConfigExtension
    {
        private static readonly string _authorityConfigKey = "Authentication:Authority";
        private static readonly string _issuerSigningConfigKey = "Authentication:IssuerSigningKey";
        private static readonly string _audienceConfigKey = "Authentication:Audience";

        public static IServiceCollection AddAuthConfig(this IServiceCollection services, IConfiguration configuration, IHostEnvironment env)
        {
            if (!IsAuthAvailable(configuration))
                return services;

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = GetEnvValueOrDefault("AUTHENTICATION_AUTHORITY", configuration[_authorityConfigKey]);
                    options.RequireHttpsMetadata = env.IsProduction();
                    options.Audience = GetEnvValueOrDefault("AUTHENTICATION_AUDIENCE", configuration[_audienceConfigKey]);
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = options.Authority,
                        ValidateAudience = true,
                        ValidAudience = options.Audience,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(GetEnvValueOrDefault("AUTHENTICATION_ISSUERSIGNINGKEY", configuration[_issuerSigningConfigKey])))
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

        private static string GetEnvValueOrDefault(string envName, string defaultValue)
        {
            var envValue = Environment.GetEnvironmentVariable(envName);

            return string.IsNullOrWhiteSpace(envValue) ? defaultValue : envValue;
        }

        private static bool IsAuthAvailable(IConfiguration configuration)
        {
            return !string.IsNullOrWhiteSpace(configuration[_authorityConfigKey])
                && !string.IsNullOrWhiteSpace(configuration[_issuerSigningConfigKey])
                && !string.IsNullOrWhiteSpace(configuration[_audienceConfigKey]);
        }
    }
}
