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

        [HttpPost("register-google")]
        public async Task<IActionResult> RegisterGoogleUser([FromForm] GoogleRegisterDto dto)
        {
            try
            {
                var user = await _usersService.RegisterGoogleUserAsync(dto);
                return Ok(user);

            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("signin-google")]
        public async Task<IActionResult> SignInGoogle([FromForm] GoogleLoginDto dto) 
        {
            try
            {
                if(string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Token))
                {
                    return BadRequest(new { error = "Email and Token are required" });
                }
                var token = await _usersService.LoginGoogleAsync(dto.Email, dto.Token);
                return Ok(new {Token = token});

            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
