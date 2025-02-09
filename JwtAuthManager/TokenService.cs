using JwtAuthManager.Interface;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JwtAuthManager
{
    internal class TokenService : ITokenService
    {
        private readonly string _jwtSecret = "";
        private readonly string _issuer = "";
        private readonly string _audience = "";

        public async Task<string> GenerateToken(string username)
        {
            var claims = new[]
            {
            new Claim(ClaimTypes.Name, username),
            new Claim("scope", "api.read")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _issuer,
                _audience,
                claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<bool> LoginUser(string username, string password)
        {
            return username == "testuser" && password == "password";
        }
    }
}
