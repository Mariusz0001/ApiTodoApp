using ApiTodoApp.Infrastructure.Database;
using ApiTodoApp.Infrastructure.Repository;

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
    }
}
