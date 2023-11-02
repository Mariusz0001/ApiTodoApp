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

        public IQueryable<PersonalTaskDto>? Get(string userId)
        {
            var personalTasks = DbContext.PersonalTasks?.Where(p => p.UserId == userId)
                .OrderByDescending(p => p.CreationDate)
                .ThenByDescending(p => p.Status)
                .Select(p => new PersonalTaskDto(p.Id, p.Name, p.CreationDate, DbContext.Users.First(u => u.Id == p.UserId).UserName, p.Status.ToString(), p.Priority.ToString(), p.Description));
            return personalTasks;
        }

        public IQueryable<PersonalTaskDto>? GetById(Guid id, string userId) =>
            DbContext.PersonalTasks?
                .Where(p => p.Id == id && p.UserId == userId)
                .Select(p => new PersonalTaskDto(p.Id, p.Name, p.CreationDate, DbContext.Users.First(u => u.Id == p.UserId).UserName, p.Status.ToString(), p.Priority.ToString(), p.Description));


        public IQueryable<PersonalTaskDto>? GetByType(string type, string userId)
        {
            var personalTasks = Get(userId);
            return personalTasks is not null ? personalTasks
                .Where(p => p.CreatedBy == userId && p.Status == type) : null;
        }

        public IQueryable<UserStatsDto>? GetUserStats(string userId) =>
            DbContext.PersonalTasks?
               .Where(p => p.UserId == userId)
               .GroupBy(task => task.Status)
               .Select(group => new UserStatsDto(group.Key.ToString(), group.Count()));

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

        public void EditDetails(EditDetailsTaskDto dto, string userId)
        {
            PersonalTask? personalTask = GetTask(dto.Id, userId);

            if (String.IsNullOrEmpty(dto.Name))
                throw new ArgumentException($"Error while task editing - {nameof(dto.Name)} is empty");

            personalTask.Name = dto.Name;
            personalTask.Description = dto.Description;
            personalTask.Status = (PersonalTaskStatus)Enum.Parse(typeof(PersonalTaskStatus), dto.Status);
            personalTask.Priority = (PersonalTaskPriority)Enum.Parse(typeof(PersonalTaskPriority), dto.Priority);

            DbContext.PersonalTasks?.Update(personalTask);
            DbContext.SaveChanges();
        }

        private PersonalTask GetTask(Guid id, string userId)
        {
            var personalTask = DbContext.PersonalTasks?.FirstOrDefault(p => p.Id == id && p.UserId == userId);

            if (personalTask is null)
                throw new ArgumentException($"Task {id} has been not found");
            return personalTask;
        }
    }
}

