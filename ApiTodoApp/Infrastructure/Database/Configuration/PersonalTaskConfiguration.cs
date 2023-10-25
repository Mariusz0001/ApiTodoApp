using ApiTodoApp.Infrastructure.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiTodoApp.Infrastructure.Database.Configuration
{
    public class PersonalTaskConfiguration : IEntityTypeConfiguration<PersonalTask>
    {
        public void Configure(EntityTypeBuilder<PersonalTask> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }
}
