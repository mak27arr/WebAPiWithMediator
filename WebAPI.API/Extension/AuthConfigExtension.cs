using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.IdentityModel.Tokens;

namespace WebAPI.API.Extension
{
    public static class AuthGoogleConfigExtension
    {
        private static readonly string _googleClientId = "Authentication:Google:ClientId";
        private static readonly string _googleClientSecret = "Authentication:Google:ClientSecret";
        private static readonly string _googleAuthority = "Authentication:Google:Authority";

        public static IServiceCollection AddAuthConfig(this IServiceCollection services, IConfiguration configuration)
        {
            if (!IsAuthAvailable(configuration))
                return services;

            var googleClientId = configuration[_googleClientId];
            var googleClientSecret = configuration[_googleClientSecret];
            var googleAuthority = configuration[_googleAuthority];

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
            })
            .AddGoogle(options =>
            {
                options.ClientId = googleClientId; 
                options.ClientSecret = googleClientSecret;
                options.CallbackPath = new PathString("/signin-google");
            })
            .AddJwtBearer(options =>
            {
                options.Authority = googleAuthority;
                options.Audience = googleClientId;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = googleAuthority,
                    ValidAudience = googleClientId,
                };
            });

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
            return !string.IsNullOrWhiteSpace(configuration[_googleClientId])
                && !string.IsNullOrWhiteSpace(configuration[_googleClientSecret])
                && !string.IsNullOrWhiteSpace(configuration[_googleAuthority]);
        }
    }
}
