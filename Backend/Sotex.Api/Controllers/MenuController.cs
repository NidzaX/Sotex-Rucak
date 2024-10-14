using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Design;
using Sotex.Api.Interfaces;

namespace Sotex.Api.Controllers
{
    [Route("api/menus")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly IOpenAIService _openAIService;

        public MenuController(IOpenAIService openAIService)
        {
            _openAIService = openAIService;
        }

        [HttpPost("upload-image-url")]
        public async Task<IActionResult> ParseImageFromUrl([FromForm] string imageUrl, [FromForm] string purpose)
        {
            try
            {
                var result = await _openAIService.ParseImageFromUrlAsync(imageUrl, purpose);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

    }
}
