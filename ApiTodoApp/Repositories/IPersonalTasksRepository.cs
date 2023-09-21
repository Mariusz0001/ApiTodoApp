using ApiTodoApp.Infrastructure.Database;

namespace ApiTodoApp.Repositories
{
    public interface IPersonalTasksRepository
    {
        IEnumerable<PersonalTask>? Get();
    }
}