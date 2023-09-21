using ApiTodoApp.Infrastructure.Database;
using ApiTodoApp.Infrastructure.Repository;

namespace ApiTodoApp.Repositories
{
    public class PersonalTasksRepository : RepositoryBase<ApplicationDbContext>
    {
        public PersonalTasksRepository(ApplicationDbContext dbContext) : base(dbContext)
        {

        }
    }
}
