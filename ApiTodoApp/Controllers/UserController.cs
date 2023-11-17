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
        private readonly ILogger<UserController> _logger;

        public UserController(IAuthenticationService authenticationService,
            UserHelper userHelper,
            ILogger<UserController> logger)
        {
            _authenticationService = authenticationService;
            _userHelper = userHelper;
            _logger = logger;
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
                _logger.LogError(aex.ToString());
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
                _logger.LogError(aex.ToString());
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
                _logger.LogError(aex.ToString());
                return BadRequest(new ErrorDto(aex.Message));
            }
        }


        [AllowAnonymous]
        [HttpPost("google")]
        public async Task<IActionResult> Google([FromBody] LoginProviderRequest request)
        {
            try
            {
                var result = await _authenticationService.LoginWithProvider(AuthenticationService.Provider.Google, request.TokenId);

                if (string.IsNullOrEmpty(result))
                    return BadRequest("Cannot authenticate user with google provider");

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                BadRequest(ex.Message);
            }
            return BadRequest();
        }
    }
}
