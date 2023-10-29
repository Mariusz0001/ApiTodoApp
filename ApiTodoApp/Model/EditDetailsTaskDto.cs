namespace ApiTodoApp.Model
{
    public record EditDetailsTaskDto(Guid Id, string Name, string? Description, string Status, string? Priority);
}
