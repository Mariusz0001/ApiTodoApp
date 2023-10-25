using ApiTodoApp.Model.User;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace ApiTodoApp.Helpers
{
    public class UserHelper
    {
        private readonly UserManager<User> _userManager;

        public UserHelper(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        public async Task<string> GetUser(ClaimsPrincipal userClaims)
        {
            try
            {
                var name = (userClaims.Identity as ClaimsIdentity)?.FindFirst("Name")?.Value;
                var user = await _userManager.FindByNameAsync(name);

                return user.Id;
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Problem with authenticating current user {ex}");
            }
        }
    }
}
