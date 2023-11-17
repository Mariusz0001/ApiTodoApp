using ApiTodoApp.Model;
using ApiTodoApp.Model.User;
using static ApiTodoApp.Infrastructure.Authentication.AuthenticationService;

namespace ApiTodoApp.Infrastructure.Authentication
{
    public interface IAuthenticationService
    {
        Task<string> Register(RegisterRequest request);
        Task<string> Login(LoginRequest request);
        Task<LoginWithProviderResponseDto> LoginWithProvider(Provider provider, string tokenId);
    }
}
