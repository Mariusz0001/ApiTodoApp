using ApiTodoApp.Model.User;
using Microsoft.AspNetCore.Identity;

namespace ApiTodoApp.Infrastructure.Authentication
{
    public class UserNameManager : IUserNameManager
    {
        private readonly UserManager<User> _userManager;
        public UserNameManager(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<string> GenerateNewUsernameAsync(string baseUsername)
        {
            var newUsername = baseUsername;
            var usernameExists = true;
            int suffix = 1;

            while (usernameExists)
            {
                newUsername = $"{baseUsername}{suffix}";
                usernameExists = await IsUsernameExistsAsync(newUsername);
                suffix++;
            }
            return newUsername;
        }

        private async Task<bool> IsUsernameExistsAsync(string username)
        {
            var ret = await _userManager.FindByNameAsync(username);
            return ret != null;
        }
    }
}
