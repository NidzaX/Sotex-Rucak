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
        private readonly UserRepo _userRepo; // testings
        public MenuController(IMenuService openAIService, UserRepo userRepo)
        {
            _menuService = openAIService;
            _userRepo = userRepo; // testing
        }

        [HttpPost("parse-and-save-menu")]
        public async Task<IActionResult> ParseAndSaveMenu(IFormFile file)
        {
            string purpose = "vision";

            if (file == null)
            {
                return BadRequest(new { error = "File is required." });
            }

            try
            {
                // Retrieve the user ID from JWT claims
                var userIdClaim = User.FindFirst("id");
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


        [HttpGet("get-menu-items")]
        public async Task<IActionResult> GetMenuItems()
        {
            var userIdClaim = User.FindFirst("id");
            if (userIdClaim == null)
            {
                return Unauthorized(new { error = "User is not authenticated." });
            }

            var userId = Guid.Parse(userIdClaim.Value);

            try
            {
                var menuItems = await _menuService.ListMenuItemsAsync(userId);
                return Ok(menuItems);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-menu-status")]
        public async Task<IActionResult> GetMenuStats(Guid userId)
        {
            try
            {
                var (isActive, isActiveTomorrow) = await _menuService.GetMenuStatusAsync(userId);
                return Ok(new {isActive, isActiveTomorrow});
            }catch(InvalidOperationException ex)
            {
                return NotFound(new { error = ex.Message });
            }catch(Exception ex)
            {
                return StatusCode(500, new {error = ex.Message});
            }
        }

    }
}
