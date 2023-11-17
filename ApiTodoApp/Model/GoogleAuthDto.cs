namespace ApiTodoApp.Model
{
    public record GoogleAuthDto(string Iss,
                                string Azp,
                                string Aud,
                                string Sub,
                                string Email,
                                string Email_verified,
                                string Nbf,
                                string Name,
                                string Picture,
                                string Given_name,
                                string Locale,
                                string Iat,
                                string Exp,
                                string Jti,
                                string Alg,
                                string Kid,
                                string Typ);
}
