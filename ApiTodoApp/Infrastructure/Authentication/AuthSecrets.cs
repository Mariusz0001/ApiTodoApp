namespace ApiTodoApp.Infrastructure.Authentication
{
    public sealed record AuthSecrets
    {
        public string? Issuer { get; init; }
        public string? Audience { get; init; }
        public double ExpirationSeconds { get; init; }
        public string? SigningKey { get; init; }
        public string? GoogleClientId { get; init; }
    }
}
