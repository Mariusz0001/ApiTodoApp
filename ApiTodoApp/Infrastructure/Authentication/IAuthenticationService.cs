using ApiTodoApp.Model.User;

namespace ApiTodoApp.Infrastructure.Authentication
{
    public interface IAuthenticationService
    {
        Task<string> Register(RegisterRequest request);
        Task<string> Login(LoginRequest request);
    }
}
