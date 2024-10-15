using Microsoft.AspNetCore.Mvc;

namespace Sotex.Api.Interfaces
{
    public interface IOpenAIService
    {
        Task<string> ParseImageFromFileAsync(IFormFile file, string purpose);
    }
}
