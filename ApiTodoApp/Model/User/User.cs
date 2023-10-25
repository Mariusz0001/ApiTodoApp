using ApiTodoApp.Infrastructure.Database.Entities;
using Microsoft.AspNetCore.Identity;

namespace ApiTodoApp.Model.User
{
    public class User : IdentityUser
    {
        public User()
        {
        }
        public List<PersonalTask>? PersonalTasks;
    }
}
