namespace ApiTodoApp.Infrastructure.Database.Entities
{
    public record PersonalTask(Guid Id,
       DateTime CreationDate,
       string UserId)
    {
        public PersonalTask(Guid id,
                            string name,
                            string userId,
                            DateTime creationDate,
                            PersonalTaskStatus status,
                            PersonalTaskPriority priority = PersonalTaskPriority.MEDIUM,
                            string? description = "") : this(id, creationDate, userId)
        {
            Name = name;
            Status = status;
            Priority = priority;
            Description = description;
        }

        public string Name { get; set; }
        public PersonalTaskStatus Status { get; set; }
        public PersonalTaskPriority? Priority { get; set; }
        public string? Description { get; set; }

    }
}
