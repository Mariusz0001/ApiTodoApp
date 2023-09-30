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
            PersonalTask? personalTask = GetTask(dto.Id);

            var status = (int)personalTask.Status < 2 ? personalTask.Status + 1 : personalTask.Status;

            personalTask.Status = status;

            DbContext.PersonalTasks?.Update(personalTask);
            DbContext.SaveChanges();            
        }

        public void Edit(EditTaskDto dto)
        {
            PersonalTask? personalTask = GetTask(dto.Id);

            if (String.IsNullOrEmpty(dto.Name))
                throw new ArgumentException($"Error while task editing - {nameof(dto.Name)} is empty");

            personalTask.Name = dto.Name;

            DbContext.PersonalTasks?.Update(personalTask);
            DbContext.SaveChanges();
        }

        private PersonalTask GetTask(Guid id)
        {
            var personalTask = DbContext.PersonalTasks?.FirstOrDefault(p => p.Id == id);

            if (personalTask is null)
                throw new ArgumentException($"Task {id} has been not found");
            return personalTask;
        }
    }
}

