using ApiTodoApp.Model;
using ApiTodoApp.Model.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiTodoApp.Infrastructure.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        public enum Provider
        {
            Google
        }

        private readonly UserManager<User> _userManager;
        private readonly IUserNameManager _userNameManager;
        private readonly AuthSecrets _authSecrets;
        private readonly IHttpClientFactory _httpClientFactory;

        public AuthenticationService(UserManager<User> userManager, IUserNameManager userNameManager, AuthSecrets secrets, IHttpClientFactory httpClientFactory)
        {
            _userManager = userManager;
            _userNameManager = userNameManager;
            _authSecrets = secrets;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> Register(RegisterRequest request)
        {
            var userByEmail = await _userManager.FindByEmailAsync(request.Email);
            if (userByEmail is not null)
                throw new ArgumentException($"User with email {request.Email} already exists.");

            var userByUsername = await _userManager.FindByNameAsync(request.UserName);
            if (userByUsername is not null)
                throw new ArgumentException($"Username { request.UserName} is already taken.");

            await RegisterUser(request.Email, request.UserName, request.Password);

            return await Login(new LoginRequest { Username = request.UserName, Password = request.Password });
        }

        public async Task<string> Login(LoginRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.Username);

            if (user is null)
            {
                user = await _userManager.FindByEmailAsync(request.Username);
            }

            if (user is null || !await _userManager.CheckPasswordAsync(user, request.Password))
            {
                throw new ArgumentException($"Unable to authenticate user {request.Username}");
            }

            var authClaims = new ClaimsIdentity(new[]
        {
            new Claim("Name", user.UserName),
            new Claim("Email", user.Email),
            new Claim("Id", user.Id)
        });

            var token = GenerateToken(authClaims);

            return token;
        }

        public async Task<LoginWithProviderResponseDto> LoginWithProvider(Provider provider, string token)
        {
            switch (provider)
            {
                case Provider.Google:
                    {
                        var httpClient = _httpClientFactory.CreateClient("Google");

                        var result = await httpClient.GetAsync($"oauth2/v3/tokeninfo?id_token={token}");
                        result.EnsureSuccessStatusCode();

                        var authDto = JsonConvert.DeserializeObject<GoogleAuthDto>(await result.Content.ReadAsStringAsync());

                        if (authDto is null)
                            throw new NullReferenceException("Cannot convert response from google on an object");

                        if (authDto.Aud != _authSecrets.GoogleClientId)
                            throw new Exception("Unauthorized token");

                        var user = await _userManager.FindByEmailAsync(authDto.Email);

                        if (user is null)
                            user = await RegisterUser(authDto);

                        return new LoginWithProviderResponseDto(GenerateToken(new ClaimsIdentity(new[]
                        {
                            new Claim("Name", user.UserName),
                            new Claim("Email", user.Email),
                            new Claim("Id", user.Id)
                        })), authDto.Picture);
                    }
                default: throw new NotImplementedException($"Logging by the provider {provider} has been not implemented.");
            }
        }

        private async Task<User> RegisterUser(GoogleAuthDto authDto)
        {
            var foundUser = await _userManager.FindByNameAsync(authDto.Name);

            var userName = authDto.Name;

            if (foundUser is not null)
                userName = await _userNameManager.GenerateNewUsernameAsync(authDto.Name);

            return await RegisterUser(authDto.Email, userName, null);
        }

        private async Task<User> RegisterUser(string email, string name, string? password)
        {
            User newUser = new()
            {
                Email = email,
                UserName = name,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = password is null ? await _userManager.CreateAsync(newUser) : await _userManager.CreateAsync(newUser, password);

            if (!result.Succeeded)
                throw new ArgumentException($"Unable to register user {name} errors: {GetErrorsText(result.Errors)}");

            var user = await _userManager.FindByEmailAsync(email);
            return user;
        }

        private string GenerateToken(ClaimsIdentity authClaims)
        {

            if (_authSecrets?.SigningKey is null)
                throw new NullReferenceException($"{nameof(_authSecrets.SigningKey)} is null,check configuration");

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_authSecrets.SigningKey);
            var expirationDate = DateTime.UtcNow.AddSeconds(_authSecrets.ExpirationSeconds);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = authClaims,
                Expires = expirationDate,
                Issuer = _authSecrets.Issuer,
                Audience = _authSecrets.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private string GetErrorsText(IEnumerable<IdentityError> errors)
        {
            return string.Join(", ", errors.Select(error => error.Description).ToArray());
        }
    }
}
