namespace DapperSamples.Authorization.Jwt.Configuration
{
    public sealed class JwtTokenConfiguration
    {
        public JwtTokenConfiguration()
        {
            //For mapping from IConfiguration
        }
        public JwtTokenConfiguration(string secretKey, string issuer, string audience)
        {
            SecretKey = secretKey;
            Issuer = issuer;
            Audience = audience;
        }

        public string SecretKey { get; init; }
        public string Issuer { get; init; }
        public string Audience { get; init; }
    }
}
