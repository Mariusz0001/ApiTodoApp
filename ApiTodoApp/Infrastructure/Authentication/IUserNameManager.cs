namespace ApiTodoApp.Infrastructure.Authentication
{
    public interface IUserNameManager
    {
        Task<string> GenerateNewUsernameAsync(string baseUsername);
    }
}