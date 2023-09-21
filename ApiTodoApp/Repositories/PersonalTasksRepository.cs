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

        public IEnumerable<PersonalTask>? Get()
        {
            var personalTasks = DbContext.PersonalTasks;
            return personalTasks is not null ? personalTasks.ToList() : null;
        }
        public Guid Add(AddTaskDto dto)
        {
            var id = Guid.NewGuid();

            DbContext.Add(new PersonalTask(id, dto.Name));
            DbContext.SaveChanges();

            return id;
        }
    }
}

