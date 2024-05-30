using AtonAPI.Services;
using AtonAPI.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace AtonAPI.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly JwtService _jwtService;

        public AuthController(IUserService userService, JwtService jwtService)
        {
            _userService = userService;
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            bool isValidUser = await _userService.ValidateUserAsync(loginModel.Login, loginModel.Password);
            if (isValidUser)
            {
                
                (string role, bool success, string errorMessage) result = await _userService.GetUserRoleAsync(loginModel.Login);
                if(result.success)
                {
                    var token = _jwtService.GenerateToken(loginModel.Login, result.role);
                    return Ok(new { Token = token });
                }
                else
                {
                    return BadRequest(result.errorMessage);
                }
            }
            else
            {
                return Unauthorized("Credentials don't match");
            }
        }
    }
}
