namespace ApiTodoApp.Model
{
    public record PersonalTaskDto(Guid Id, String Name, DateTime CreationDate, string CreatedBy, string Status, string Priority, string? Description);
}
