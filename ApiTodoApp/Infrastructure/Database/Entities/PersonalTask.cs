namespace ApiTodoApp.Infrastructure.Database.Entities
{
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
}
