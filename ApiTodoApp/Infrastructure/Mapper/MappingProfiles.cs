using ApiTodoApp.Infrastructure.Database.Entities;
using ApiTodoApp.Model;
using AutoMapper;

namespace ApiTodoApp.Infrastructure.Mapper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<PersonalTask, PersonalTaskDto>();
        }
    }
}
