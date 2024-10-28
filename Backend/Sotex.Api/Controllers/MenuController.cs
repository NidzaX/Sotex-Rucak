using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sotex.Api.Interfaces;
using Sotex.Api.Repo;

namespace Sotex.Api.Controllers
{
    [Route("api/menus")]
    [ApiController]
    [Authorize]
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
        public async Task<IActionResult> ParseAndSaveMenu(IFormFile file, [FromForm] string purpose)
        {
            if (file == null || string.IsNullOrEmpty(purpose))
            {
                return BadRequest(new { error = "File and purpose are required." });
            }

            try
            {
                // Retrieve the user ID from JWT claims
                var userIdClaim = User.FindFirst("sub");
                if (userIdClaim == null)
                {
                    return Unauthorized(new { error = "User is not authenticated." });
                }

                var userId = Guid.Parse(userIdClaim.Value);

                // Pass the user ID to the service method
                var menu = await _menuService.ParseAndSaveMenuFromFileAsync(file, purpose, userId);
                return Ok(new { message = "Menu saved successfully", menu });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }


    }
}
