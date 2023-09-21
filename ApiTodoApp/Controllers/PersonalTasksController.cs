using ApiTodoApp.Model;
using ApiTodoApp.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace ApiTodoApp.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("[controller]")]
    //[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
    public class PersonalTasksController : ControllerBase
    {
        private readonly ILogger<PersonalTasksController> _logger;
        private readonly IPersonalTasksRepository _repository;
        private readonly IMapper _mapper;

        public PersonalTasksController(ILogger<PersonalTasksController> logger,
                                       IPersonalTasksRepository repository,
                                       IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public IEnumerable<PersonalTaskDto> Get()
        {
            _logger.LogInformation("METHOD: Get, PersonalTasksController");//todo loging request response
            var response = _repository.Get();

            return response?.Count() > 0 ? _mapper.Map<IEnumerable<PersonalTaskDto>>(response) : new List<PersonalTaskDto>();
        }
    }
}