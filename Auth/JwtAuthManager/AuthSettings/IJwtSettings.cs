namespace JwtAuthManager.AuthSettings
{
    internal interface IJwtSettings
    {
        string Audience { get; }
        string Authority { get; }
        string IssuerSigningKey { get; }
    }
}