namespace ApiTodoApp.Infrastructure.Authentication
{
    public sealed record AuthSecrets
    {
        public string UserName { get; init; }
        public string Password { get; init; }
        public string Issuer { get; init; }
        public string Audience { get; init; }
        public double ExpirationSeconds { get; init; }
        public string SigningKey { get; init; }
    }
}
