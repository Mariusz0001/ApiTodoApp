using Microsoft.EntityFrameworkCore;

namespace ApiTodoApp.Infrastructure.Database
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        public DbSet<PersonalTask>? PersonalTasks { get; set; }
    }

    public record PersonalTask(Guid Id,
        DateTime CreationDate)
    {
        public PersonalTask(Guid id, string? name, DateTime creationDate, PersonalTaskStatus status) : this(id, creationDate)
        {
            Name = name;
            Status = status;
        }

        public string? Name { get; set; }
        public PersonalTaskStatus Status { get; set; }
    }

    public enum PersonalTaskStatus
    {
        TO_DO = 0,
        IN_PROGRESS = 1,
        COMPLETE = 2
    }
}
