using ApiTodoApp.Model.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiTodoApp.Infrastructure.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<User> _userManager;
        private readonly AuthSecrets _authSecrets;

        public AuthenticationService(UserManager<User> userManager, AuthSecrets secrets)
        {
            _userManager = userManager;
            _authSecrets = secrets;
        }

        public async Task<string> Register(RegisterRequest request)
        {
            var userByEmail = await _userManager.FindByEmailAsync(request.Email);
            if (userByEmail is not null)
                throw new ArgumentException($"User with email {request.Email} already exists.");

            var userByUsername = await _userManager.FindByNameAsync(request.UserName);
            if (userByUsername is not null)
                throw new ArgumentException($"Username { request.UserName} is already taken.");

            User user = new()
            {
                Email = request.Email,
                UserName = request.UserName,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                throw new ArgumentException($"Unable to register user {request.UserName} errors: {GetErrorsText(result.Errors)}");
            }

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
            new Claim("Email", user.Email)
        });


            var tokenHandler = new JwtSecurityTokenHandler();
            var token = GenerateToken(authClaims);


            return token;
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
