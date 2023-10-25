namespace ApiTodoApp.Infrastructure.Database.Entities
{
    public record PersonalTask(Guid Id,
       DateTime CreationDate,
       string userId)
    {
        public PersonalTask(Guid id, string? name, string userId, DateTime creationDate, PersonalTaskStatus status) : this(id, creationDate, userId)
        {
            Name = name;
            Status = status;
        }

        public string? Name { get; set; }
        public PersonalTaskStatus Status { get; set; }
    }
}
