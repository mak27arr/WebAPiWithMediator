using JwtAuthManager.AuthSettings;
using JwtAuthManager.Interface;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JwtAuthManager
{
    internal class TokenService : ITokenService
    {
        private readonly IJwtSettings _settings;

        public TokenService(IJwtSettings settings)
        {
            _settings = settings;
        }

        public async Task<string> GenerateToken(string username)
        {
            var claims = new[]
            {
            new Claim(ClaimTypes.Name, username),
            new Claim("scope", "api.read")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.IssuerSigningKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _settings.Authority,
                _settings.Audience,
                claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<ClaimsPrincipal> ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var key = Encoding.UTF8.GetBytes(_settings.IssuerSigningKey);
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = _settings.Authority,
                    ValidAudience = _settings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
                return principal;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> LoginUser(string username, string password)
        {
            return username == "testuser" && password == "password";
        }
    }
}
