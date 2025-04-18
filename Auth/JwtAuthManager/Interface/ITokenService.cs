using System.Security.Claims;

namespace JwtAuthManager.Interface
{
    public interface ITokenService
    {
        Task<string> GenerateToken(string username);

        Task<ClaimsPrincipal?> ValidateToken(string token);

        Task<bool> LoginUser(string username, string password);
    }
}