using ApiTodoApp.Infrastructure.Database;
using ApiTodoApp.Model;

namespace ApiTodoApp.Repositories
{
    public interface IPersonalTasksRepository
    {
        IEnumerable<PersonalTask>? Get();
        Guid Add(AddTaskDto dto);
    }
}