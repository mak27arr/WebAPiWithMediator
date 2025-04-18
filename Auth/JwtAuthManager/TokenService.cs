using JwtAuthManager.AuthSettings;
using JwtAuthManager.Interface;
using Microsoft.IdentityModel.JsonWebTokens;
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

        public Task<string> GenerateToken(string username)
        {
            var claimsIdentity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim("scope", "api.read"),
                new Claim("aud", _settings.Audience ?? string.Empty),
                new Claim("iss", _settings.Authority ?? string.Empty)
            });

            if (string.IsNullOrEmpty(_settings.IssuerSigningKey))
                return Task.FromResult(string.Empty);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.IssuerSigningKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor {
                Subject = claimsIdentity,
                NotBefore = DateTime.UtcNow,
                Expires = DateTime.Now.AddHours(1),
                SigningCredentials = credentials
            };

            var handler = new JsonWebTokenHandler();
            return Task.FromResult(handler.CreateToken(tokenDescriptor));
        }

        public Task<ClaimsPrincipal?> ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                if (string.IsNullOrWhiteSpace(_settings.IssuerSigningKey))
                    return Task.FromResult(null as ClaimsPrincipal);

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
                return Task.FromResult<ClaimsPrincipal?>(principal);
            }
            catch (Exception)
            {
                return Task.FromResult(null as ClaimsPrincipal);
            }
        }

        public Task<bool> LoginUser(string username, string password)
        {
            return Task.FromResult(username == "testuser" && password == "password");
        }
    }
}
