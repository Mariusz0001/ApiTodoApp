using ApiTodoApp.Infrastructure.Database.Entities;
using ApiTodoApp.Model.User;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ApiTodoApp.Infrastructure.Database
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        public DbSet<PersonalTask>? PersonalTasks { get; set; }
    }
}
