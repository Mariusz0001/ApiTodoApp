using ApiTodoApp.Infrastructure.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApiTodoApp.Infrastructure.Database
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        public DbSet<PersonalTask>? PersonalTasks { get; set; }
    }
}
