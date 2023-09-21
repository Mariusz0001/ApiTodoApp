using Microsoft.EntityFrameworkCore;

namespace ApiTodoApp.Infrastructure.Database
{
    public class ApplicationDbContext : DbContext
    {
        static readonly string connectionString = "Server=localhost; User ID=root; Password=Sklepikarz123!; Database=todoapp-db";//todo move cn to appsettings or vault

        public DbSet<PersonalTask>? PersonalTasks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }
    }

    public record PersonalTask(Guid Id, string? Name, DateTime CreationDate, PersonalTaskStatus Status);

    public enum PersonalTaskStatus
    {
        TO_DO,
        IN_PROGRESS,
        COMPLETE
    }
}
