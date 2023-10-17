namespace ApiTodoApp.Infrastructure.Authentication
{
    public record AuthSecrets(string UserName, string Password, string Issuer, string Audience, double ExpirationSeconds, string SigningKey);
}
