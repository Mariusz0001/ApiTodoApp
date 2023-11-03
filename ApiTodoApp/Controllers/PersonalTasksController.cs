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
            var response = _repository.Get(await _userHelper.GetUserId(User))?.ToList();

            return response?.Count > 0 ? response : new List<PersonalTaskDto>();
        }

        [HttpGet("{type}")]
        public async Task<IEnumerable<PersonalTaskDto>> GetAsync([FromRoute] string type)
        {
            _logger.LogInformation("METHOD: GET, PersonalTasksController");//todo loging request response
            var response = _repository.GetByType(type, await _userHelper.GetUserId(User));

            return response?.Count() > 0 ? response : new List<PersonalTaskDto>();
        }

        [HttpGet("byId/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PersonalTaskDto>> GetByIdAsync([FromRoute] string id)
        {
            _logger.LogInformation("METHOD: GET, PersonalTasksController");//todo loging request response

            _ = Guid.TryParse(id, out var guid);

            if (guid == Guid.Empty)
                return BadRequest("Wrong id");

            var response = _repository.GetById(Guid.Parse(id), await _userHelper.GetUserId(User))?.FirstOrDefault();

            if (response is not null)
                return Ok(_mapper.Map<PersonalTaskDto>(response));

            return NotFound();
        }

        [HttpGet("userStats")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<UserStatsDto>>> UserStats()
        {
            _logger.LogInformation("METHOD: GET, PersonalTasksController");//todo loging request response

            var response = _repository.GetUserStats(await _userHelper.GetUserId(User));

            if (response is not null)
                return Ok(response);

            return NotFound();
        }


        [HttpPost("add")]
        public async Task<Guid> AddTaskAsync([FromBody] AddTaskDto addTaskDto)
        {
            _logger.LogInformation("METHOD: POST, PersonalTasksController");//todo loging request response

            var id = _repository.Add(addTaskDto, await _userHelper.GetUserId(User));

            return id;
        }


        [HttpPost("move")]
        public async Task MoveTaskAsync([FromBody] MoveTaskDto dto)
        {
            _logger.LogInformation("METHOD: POST, PersonalTasksController");//todo loging request response
            _repository.Move(dto, await _userHelper.GetUserId(User));
        }

        [HttpPost("edit")]
        public async Task EditTaskAsync([FromBody] EditTaskDto dto)
        {
            _logger.LogInformation("METHOD: POST, PersonalTasksController");//todo loging request response
            _repository.Edit(dto, await _userHelper.GetUserId(User));
        }

        [HttpPost("edit-details")]
        public async Task EditDetailsTaskAsync([FromBody] EditDetailsTaskDto dto)
        {
            _logger.LogInformation("METHOD: POST, PersonalTasksController");//todo loging request response
            _repository.EditDetails(dto, await _userHelper.GetUserId(User));
        }
    }
}