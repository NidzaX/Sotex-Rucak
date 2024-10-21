using Microsoft.AspNetCore.Mvc;
using Sotex.Api.Interfaces;
using Sotex.Api.Repo;

namespace Sotex.Api.Controllers
{
    [Route("api/menus")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly IMenuService _menuService;
        private readonly UserRepo _userRepo; // testing

        public MenuController(IMenuService openAIService, UserRepo userRepo)
        {
            _menuService = openAIService;
            _userRepo = userRepo; // testing
        }

        [HttpPost("parse-and-save-menu")]
        public async Task<IActionResult> ParseAndSaveMenu(IFormFile file, [FromForm] string purpose, [FromForm] string email)
        {
            if (file == null || string.IsNullOrEmpty(purpose))
            {
                return BadRequest(new { error = "File and purpose are required." });
            }

            try
            {
                //var userIdClaim = User.FindFirst("sub"); // I need to modify this using JWT.io when the user is logged in.....
                //if (userIdClaim == null)
                //{
                //    return Unauthorized(new { error = "User is not authenticated." });
                //}

                //var userId = Guid.Parse(userIdClaim.Value); 
                var user = await _userRepo.FindByEmailAsync(email);
                if (user == null)
                {
                    return NotFound(new { error = "User not found" });
                }

                var menu = await _menuService.ParseAndSaveMenuFromFileAsync(file, purpose, user.Id);
                return Ok(new { message = "Menu saved successfully", menu });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

    }
}
