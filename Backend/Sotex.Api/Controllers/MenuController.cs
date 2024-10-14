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

        [HttpPost("upload-image")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            try
            {
                var parsedContent = await _openAIService.ParseImageContentAsync(file);
                return Ok(new { message = "Image content parsed successfully", content = parsedContent });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

    }
}
