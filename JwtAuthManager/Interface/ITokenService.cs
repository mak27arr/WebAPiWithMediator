namespace JwtAuthManager.Interface
{
    public interface ITokenService
    {
        Task<string> GenerateToken(string username);

        Task<bool> LoginUser(string username, string password);
    }
}