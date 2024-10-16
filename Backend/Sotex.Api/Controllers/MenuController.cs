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

        [HttpPost("parse-image")]
        public async Task<IActionResult> ParseImage(IFormFile file, [FromForm] string purpose)
        {
            if (file == null || string.IsNullOrEmpty(purpose))
            {
                return BadRequest(new { error = "File and purpose are required." });
            }

            try
            {
                var description = await _menuService.ParseImageFromFileAsync(file, purpose);
                return Ok(new { message = "Image parsed successfully", description });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
