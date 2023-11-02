using ApiTodoApp.Model;

namespace ApiTodoApp.Repositories
{
    public interface IPersonalTasksRepository
    {
        IQueryable<PersonalTaskDto>? Get(string userId);
        IQueryable<PersonalTaskDto>? GetByType(string type, string userId);
        IQueryable<PersonalTaskDto>? GetById(Guid id, string userId);
        IQueryable<UserStatsDto>? GetUserStats(string userId);
        Guid Add(AddTaskDto dto, string userId);
        void Move(MoveTaskDto dto, string userId);
        void Edit(EditTaskDto dto, string userId);
        void EditDetails(EditDetailsTaskDto dto, string userId);
    }
}