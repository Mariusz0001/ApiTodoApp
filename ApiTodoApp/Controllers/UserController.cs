using ApiTodoApp.Helpers;
using ApiTodoApp.Infrastructure.Authentication;
using ApiTodoApp.Model;
using ApiTodoApp.Model.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiTodoApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly UserHelper _userHelper;

        public UserController(IAuthenticationService authenticationService,
            UserHelper userHelper)
        {
            _authenticationService = authenticationService;
            _userHelper = userHelper;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var response = await _authenticationService.Login(request);

                return Ok(new UserDto(response));
            }
            catch (ArgumentException aex)
            {
                return BadRequest(new ErrorDto(aex.Message));
            }
        }

        [AllowAnonymous]
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                var response = await _authenticationService.Register(request);
                return Ok(new UserDto(response));
            }
            catch (ArgumentException aex)
            {
                return BadRequest(new ErrorDto(aex.Message));
            }
        }

        [Authorize]
        [HttpGet("profile")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetProfile()
        {
            try
            {
                var user = await _userHelper.GetUser(User);

                return Ok(new ProfileDto(user.UserName, user.Email));
            }
            catch (ArgumentException aex)
            {
                return BadRequest(new ErrorDto(aex.Message));
            }
        }
    }
}
