using ApiTodoApp.Infrastructure.Database;
using ApiTodoApp.Model;
using AutoMapper;

namespace ApiTodoApp.Infrastructure.Mapper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<IEnumerable<PersonalTask>, IEnumerable<PersonalTaskDto>>();
        }
    }
}
