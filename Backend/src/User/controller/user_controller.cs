namespace Backend.src.User.controller
{
    using Backend.Error;
    using Backend.src.User.dtos;
    using Backend.src.User.Interface;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] User_Register_DTO userDto)
        {
            var result = await _userService.RegisterUserAsync(userDto);
            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, new { error = result.Error });
            }
            return Ok(result.StatusCode);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] User_Login_DTO loginDto)
        {
            var result = await _userService.LoginUserAsync(loginDto);
            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, new { error = result.Error });
            }
            return Ok(new { token = result.Value });
        }
    }
}