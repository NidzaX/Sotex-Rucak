using Microsoft.AspNetCore.Mvc;
using Sotex.Api.Interfaces;

namespace Sotex.Api.Controllers
{
    [Route("api/menus")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly IMenuService _menuService;

        public MenuController(IMenuService openAIService)
        {
            _menuService = openAIService;
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
                var menu = await _menuService.ParseAndSaveMenuFromFileAsync(file, purpose);
                return Ok(new { message = "Menu saved successfully", menu });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
