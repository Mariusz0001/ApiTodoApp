using ApiTodoApp.Model;
using ApiTodoApp.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiTodoApp.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]

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
            _logger.LogInformation("METHOD: GET, PersonalTasksController");//todo loging request response
            var response = _repository.Get();

            return response?.Count() > 0 ? _mapper.Map<IEnumerable<PersonalTaskDto>>(response) : new List<PersonalTaskDto>();
        }

        [HttpGet("{type}")]
        public IEnumerable<PersonalTaskDto> Get([FromRoute] string type)
        {
            _logger.LogInformation("METHOD: GET, PersonalTasksController");//todo loging request response
            var response = _repository.GetByType(type);

            return response?.Count() > 0 ? _mapper.Map<IEnumerable<PersonalTaskDto>>(response) : new List<PersonalTaskDto>();
        }


        [HttpPost("add")]
        public Guid AddTask([FromBody] AddTaskDto addTaskDto)
        {
            _logger.LogInformation("METHOD: POST, PersonalTasksController");//todo loging request response

            var id = _repository.Add(addTaskDto);

            return id;
        }

        [HttpPost("move")]
        public void MoveTask([FromBody] MoveTaskDto dto)
        {
            _logger.LogInformation("METHOD: POST, PersonalTasksController");//todo loging request response
            _repository.Move(dto);
        }

        [HttpPost("edit")]
        public void EditTask([FromBody] EditTaskDto dto)
        {
            _logger.LogInformation("METHOD: POST, PersonalTasksController");//todo loging request response
            _repository.Edit(dto);
        }
    }
}