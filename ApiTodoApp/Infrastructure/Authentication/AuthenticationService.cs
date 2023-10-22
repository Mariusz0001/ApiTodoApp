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
            var userByUsername = await _userManager.FindByNameAsync(request.UserName);
            if (userByEmail is not null || userByUsername is not null)
            {
                throw new ArgumentException($"User with email {request.Email} or username {request.UserName} already exists.");
            }

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

            var authClaims = new List<Claim>
        {
            new(ClaimTypes.Name, user.UserName),
            new(ClaimTypes.Email, user.Email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

            var token = GetToken(authClaims);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private JwtSecurityToken GetToken(IEnumerable<Claim> authClaims)
        {
            if (_authSecrets?.SigningKey is null)
                throw new NullReferenceException($"{nameof(_authSecrets.SigningKey)} is null,check configuration");

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authSecrets.SigningKey));

            var token = new JwtSecurityToken(
                // issuer: _authSecrets.Issuer,
                //  audience: _authSecrets.Audience,
                expires: DateTime.Now.Add(TimeSpan.FromSeconds(_authSecrets.ExpirationSeconds)),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256));

            return token;
        }

        private string GetErrorsText(IEnumerable<IdentityError> errors)
        {
            return string.Join(", ", errors.Select(error => error.Description).ToArray());
        }
    }
}
