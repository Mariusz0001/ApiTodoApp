using ApiTodoApp.Infrastructure.Database;
using ApiTodoApp.Model;

namespace ApiTodoApp.Repositories
{
    public interface IPersonalTasksRepository
    {
        IQueryable<PersonalTask>? Get();
        IQueryable<PersonalTask>? GetByType(string type);
        Guid Add(AddTaskDto dto);
        void Move(MoveTaskDto dto);
    }
}