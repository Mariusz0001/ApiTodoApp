using ApiTodoApp.Helpers;
using ApiTodoApp.Model;
using ApiTodoApp.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiTodoApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]

    public class PersonalTasksController : ControllerBase
    {
        private readonly ILogger<PersonalTasksController> _logger;
        private readonly IPersonalTasksRepository _repository;
        private readonly IMapper _mapper;
        private readonly UserHelper _userHelper;

        public PersonalTasksController(ILogger<PersonalTasksController> logger,
                                       IPersonalTasksRepository repository,
                                       IMapper mapper,
                                       UserHelper userHelper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
            _userHelper = userHelper;
        }

        [HttpGet]
        public async Task<IEnumerable<PersonalTaskDto>> Get()
        {
            _logger.LogInformation("METHOD: GET, PersonalTasksController");//todo loging request response
            var response = _repository.Get(await _userHelper.GetUser(User));

            return response?.Count() > 0 ? _mapper.Map<IEnumerable<PersonalTaskDto>>(response) : new List<PersonalTaskDto>();
        }

        [HttpGet("{type}")]
        public async Task<IEnumerable<PersonalTaskDto>> GetAsync([FromRoute] string type)
        {
            _logger.LogInformation("METHOD: GET, PersonalTasksController");//todo loging request response
            var response = _repository.GetByType(type, await _userHelper.GetUser(User));

            return response?.Count() > 0 ? _mapper.Map<IEnumerable<PersonalTaskDto>>(response) : new List<PersonalTaskDto>();
        }


        [HttpPost("add")]
        public async Task<Guid> AddTaskAsync([FromBody] AddTaskDto addTaskDto)
        {
            _logger.LogInformation("METHOD: POST, PersonalTasksController");//todo loging request response

            var id = _repository.Add(addTaskDto, await _userHelper.GetUser(User));

            return id;
        }


        [HttpPost("move")]
        public async Task MoveTaskAsync([FromBody] MoveTaskDto dto)
        {
            _logger.LogInformation("METHOD: POST, PersonalTasksController");//todo loging request response
            _repository.Move(dto, await _userHelper.GetUser(User));
        }

        [HttpPost("edit")]
        public async Task EditTaskAsync([FromBody] EditTaskDto dto)
        {
            _logger.LogInformation("METHOD: POST, PersonalTasksController");//todo loging request response
            _repository.Edit(dto, await _userHelper.GetUser(User));
        }
    }
}