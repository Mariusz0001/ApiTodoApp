using ApiTodoApp.Infrastructure.Database;
using ApiTodoApp.Infrastructure.Repository;
using ApiTodoApp.Model;

namespace ApiTodoApp.Repositories
{
    public class PersonalTasksRepository : RepositoryBase<ApplicationDbContext>, IPersonalTasksRepository
    {
        public PersonalTasksRepository(ApplicationDbContext dbContext) : base(dbContext)
        {

        }

        public IQueryable<PersonalTask>? Get()
        {
            var personalTasks = DbContext.PersonalTasks;
            return personalTasks is not null ? personalTasks.OrderByDescending(p => p.CreationDate).ThenByDescending(p => p.Status) : null;
        }

        public IQueryable<PersonalTask>? GetByType(string type)
        {
            var personalTasks = this.Get();
            return personalTasks is not null ? personalTasks
                .Where(p => p.Status == (PersonalTaskStatus)Enum.Parse(typeof(PersonalTaskStatus), type)) : null;
        }

        public Guid Add(AddTaskDto dto)
        {
            var id = Guid.NewGuid();

            DbContext.Add(new PersonalTask(id, dto.Name, DateTime.Now, (PersonalTaskStatus)Enum.Parse(typeof(PersonalTaskStatus), dto.Type)));
            DbContext.SaveChanges();

            return id;
        }

        public void Move(MoveTaskDto dto)
        {
            var personalTask = DbContext.PersonalTasks?.FirstOrDefault(p => p.Id == dto.Id);

            if (personalTask is null)
                throw new ArgumentException($"Task {dto.Id} has been not found");

            personalTask.Status = (PersonalTaskStatus)Enum.Parse(typeof(PersonalTaskStatus), dto.Status);

            DbContext.PersonalTasks?.Update(personalTask);
            DbContext.SaveChanges();            
        }
    }
}

