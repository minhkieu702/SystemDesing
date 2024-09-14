using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UberSystem.Domain.Entities;
using UberSystem.Domain.Interfaces.Services;
using UberSystem.Domain.RequestModels;

namespace UberSystem.Api.Authentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private IUserService _userService;
        public UserController(ILogger<UserController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }
        [HttpPost("Register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Register([FromBody] UserRequestModel model)
        {
            try
            {
                var user = new User();
                user.Email = model.Email;
                user.Password = model.Password;
                user.UserName = model.UserName;
                return _userService.Add(user).Result == 0 ? BadRequest("This email is already used.") : Ok("Register successfully!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetAll")]
        public IActionResult GetUsers() => Ok(_userService.GetAll().Result);
    }
}
