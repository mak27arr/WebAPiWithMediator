namespace JwtAuthManager.AuthSettings
{
    internal class JwtSettings : IJwtSettings
    {
        public string IssuerSigningKey { get; set; }

        public string Authority { get; set; }

        public string Audience { get; set; }
    }
}
