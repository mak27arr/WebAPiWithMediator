using JwtAuthManager.AuthSettings;
using JwtAuthManager.Handler;
using JwtAuthManager.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JwtAuthManager.Extension
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureMediator(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<MediatorRegister>());
            return services;
        }

        public static IServiceCollection AddJwtAuthManager(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("Authentication").Get<JwtSettings>() ?? new JwtSettings();
            UpdateJwtSettingFromEnv(jwtSettings);
            services.AddSingleton<IJwtSettings>(jwtSettings);
            services.AddScoped<ITokenService, TokenService>();
            return services;
        }

        private static void UpdateJwtSettingFromEnv(JwtSettings jwtSettings)
        {
            var authority = Environment.GetEnvironmentVariable("AUTHENTICATION_AUTHORITY");
            if (!string.IsNullOrWhiteSpace(authority))
                jwtSettings.Authority = authority;

            var issuerSigningKey = Environment.GetEnvironmentVariable("AUTHENTICATION_ISSUERSIGNINGKEY");
            if (!string.IsNullOrWhiteSpace(issuerSigningKey))
                jwtSettings.IssuerSigningKey = issuerSigningKey;

            var audience = Environment.GetEnvironmentVariable("AUTHENTICATION_AUDIENCE");
            if (!string.IsNullOrWhiteSpace(audience))
                jwtSettings.Audience = audience;
        }
    }
}
