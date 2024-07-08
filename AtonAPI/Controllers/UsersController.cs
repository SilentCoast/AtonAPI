using AtonAPI.Data.Models;
using AtonAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AtonAPI.Controllers
{
    //TODO: configure proper response codes
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }
        /// <summary>
        /// Returns the login of the user sending the request
        /// </summary>
        private string GetCurrentLogin()
        {
            string loginClaim = User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").ToString();
            var splitted = loginClaim.Split(':');
            return splitted[splitted.Length - 1].Trim();
        }

        [HttpPost("create")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Create(UserCreateModel model)
        {
            string createdByLogin = GetCurrentLogin();

            var result = await _userService.CreateUserAsync(model, createdByLogin);
            if (!result.success)
            {
                return BadRequest(result.errorMessage);
            }
            return Ok();
        }

        [HttpPut("update/details")]
        [Authorize]
        public async Task<IActionResult> UpdateDetails([Required] string login, string? name = null, int? gender = null, DateTime? birthDay = null)
        {
            var result = await _userService.UpdateUserInfoAsync(login, GetCurrentLogin(), name, gender, birthDay);
            if (!result.success)
            {
                return BadRequest(result.errorMessage);
            }
            return Ok();
        }

        [HttpPut("update/password")]
        [Authorize]
        public async Task<IActionResult> UpdatePassword(string login, string newPassword)
        {
            //TODO: use encryption for password
            //TODO: ask for old password for extra verification
            var result = await _userService.UpdateUserPasswordAsync(login, newPassword, GetCurrentLogin());
            if (!result.success)
            {
                return BadRequest(result.errorMessage);
            }
            return Ok();
        }

        [HttpPut("update/login")]
        [Authorize]
        public async Task<IActionResult> UpdateLogin(string login, string newLogin)
        {
            var result = await _userService.UpdateUserLoginAsync(login, newLogin, GetCurrentLogin());
            if (!result.success)
            {
                return BadRequest(result.errorMessage);
            }
            return Ok();
        }

        [HttpGet("active-users")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> GetActiveUsers()
        {
            var result = await _userService.GetAllActiveUsersAsync();
            if (!result.success)
            {
                return BadRequest(result.errorMessage);
            }
            return Ok(result.users);
        }

        [HttpGet("{login}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> GetUserByLogin(string login)
        {
            var result = await _userService.GetUserDTOByLoginAsync(login);
            if (!result.success)
            {
                return BadRequest(result.errorMessage);
            }
            return Ok(result.userDTO);
        }
        [HttpGet("user")]
        [Authorize]
        public async Task<IActionResult> GetUserByLoginAndPassword(string login, string password)
        {
            var result = await _userService.GetUserByLoginAndPasswordAsync(login, password, GetCurrentLogin());
            if (!result.success)
            {
                return BadRequest(result.errorMessage);
            }
            return Ok(result.user);
        }


        [HttpGet("older-than/{age}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> GetUsersOlderThan(int age)
        {
            var result = await _userService.GetUsersOlderThanAsync(age);
            if (!result.success)
            {
                return BadRequest(result.errorMessage);
            }
            return Ok(result.users);
        }

        [HttpDelete("delete/{login}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> DeleteUser(string login, bool softDelete = true)
        {
            var result = await _userService.DeleteUserAsync(login, softDelete, GetCurrentLogin());
            if (!result.success)
            {
                return BadRequest(result.errorMessage);
            }
            return Ok();
        }

        [HttpPut("restore/{login}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> RestoreUser(string login)
        {
            var result = await _userService.RestoreUserAsync(login, GetCurrentLogin());
            if (!result.success)
            {
                return BadRequest(result.errorMessage);
            }
            return Ok();
        }
    }
}
