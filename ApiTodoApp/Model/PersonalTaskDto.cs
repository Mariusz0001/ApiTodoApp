namespace ApiTodoApp.Model
{
    public record PersonalTaskDto(Guid Id, String Name, DateTime CreationDate, string Status);
}
