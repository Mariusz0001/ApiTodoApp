using ApiTodoApp.Infrastructure.Database.Entities;
using ApiTodoApp.Model;

namespace ApiTodoApp.Repositories
{
    public interface IPersonalTasksRepository
    {
        IQueryable<PersonalTask>? Get(string userId);
        IQueryable<PersonalTask>? GetByType(string type, string userId);
        Guid Add(AddTaskDto dto, string userId);
        void Move(MoveTaskDto dto, string userId);
        void Edit(EditTaskDto dto, string userId);
    }
}