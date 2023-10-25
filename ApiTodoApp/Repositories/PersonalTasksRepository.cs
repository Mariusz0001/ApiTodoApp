using ApiTodoApp.Infrastructure.Database;
using ApiTodoApp.Infrastructure.Database.Entities;
using ApiTodoApp.Infrastructure.Repository;
using ApiTodoApp.Model;

namespace ApiTodoApp.Repositories
{
    public class PersonalTasksRepository : RepositoryBase<ApplicationDbContext>, IPersonalTasksRepository
    {
        public PersonalTasksRepository(ApplicationDbContext dbContext) : base(dbContext)
        {

        }

        public IQueryable<PersonalTask>? Get(string userId)
        {
            var personalTasks = DbContext.PersonalTasks?.Where(p => p.userId == userId);
            return personalTasks is not null ? personalTasks.OrderByDescending(p => p.CreationDate).ThenByDescending(p => p.Status) : null;
        }

        public IQueryable<PersonalTask>? GetByType(string type, string userId)
        {
            var personalTasks = Get(userId);
            return personalTasks is not null ? personalTasks
                .Where(p => p.userId == userId && p.Status == (PersonalTaskStatus)Enum.Parse(typeof(PersonalTaskStatus), type)) : null;
        }

        public Guid Add(AddTaskDto dto, string userId)
        {
            var id = Guid.NewGuid();

            DbContext.Add(new PersonalTask(id, dto.Name, userId, DateTime.Now, (PersonalTaskStatus)Enum.Parse(typeof(PersonalTaskStatus), dto.Type)));
            DbContext.SaveChanges();

            return id;
        }

        public void Move(MoveTaskDto dto, string userId)
        {
            PersonalTask? personalTask = GetTask(dto.Id, userId);

            var status = (int)personalTask.Status < 2 ? personalTask.Status + 1 : personalTask.Status;

            personalTask.Status = status;

            DbContext.PersonalTasks?.Update(personalTask);
            DbContext.SaveChanges();
        }

        public void Edit(EditTaskDto dto, string userId)
        {
            PersonalTask? personalTask = GetTask(dto.Id, userId);

            if (String.IsNullOrEmpty(dto.Name))
                throw new ArgumentException($"Error while task editing - {nameof(dto.Name)} is empty");

            personalTask.Name = dto.Name;

            DbContext.PersonalTasks?.Update(personalTask);
            DbContext.SaveChanges();
        }

        private PersonalTask GetTask(Guid id, string userId)
        {
            var personalTask = DbContext.PersonalTasks?.FirstOrDefault(p => p.Id == id && p.userId == userId);

            if (personalTask is null)
                throw new ArgumentException($"Task {id} has been not found");
            return personalTask;
        }
    }
}

