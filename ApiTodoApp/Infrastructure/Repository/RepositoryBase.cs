using Microsoft.EntityFrameworkCore;

namespace ApiTodoApp.Infrastructure.Repository
{
    public class RepositoryBase<TDbContext> where TDbContext:DbContext
    {
        protected TDbContext DbContext { get; }

        protected RepositoryBase(TDbContext dbContext)
        {
            DbContext = dbContext;
        }
    }
}
