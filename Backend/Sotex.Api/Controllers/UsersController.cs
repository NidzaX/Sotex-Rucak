using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Sotex.Api.Interfaces;
using System.Security.Claims;
using Sotex.Api.Dto.UserDto;

namespace Sotex.Api.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        [HttpPost("signin-google")]
        public async Task<IActionResult> SignInGoogle([FromForm] GoogleLoginDto request) 
        {
            
            var userId = await _usersService.LoginGoogleAsync(request.Email, request.Token);

            
            if (userId != null)
            {
                return Ok(new { UserId = userId });
            }
            else
            {
                return BadRequest("Login failed.");
            }
        }
    }
}
