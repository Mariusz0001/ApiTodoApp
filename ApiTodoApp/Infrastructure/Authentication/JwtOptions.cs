namespace ApiTodoApp.Infrastructure.Authentication
{
    public class JwtOptions
    {
        public JwtOptions()
        {

        }

        public JwtOptions(string issuer, string audience, string signingKey, int expirationSeconds)
        {
            Issuer = issuer;
            Audience = audience;
            SigningKey = signingKey;
            ExpirationSeconds = expirationSeconds;
        }

        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string SigningKey { get; set; }
        public int ExpirationSeconds { get; set; }
    }
}
