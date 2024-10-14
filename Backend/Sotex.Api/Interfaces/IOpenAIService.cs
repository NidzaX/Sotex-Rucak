namespace Sotex.Api.Interfaces
{
    public interface IOpenAIService
    {
        Task<string> ParseImageContentAsync(IFormFile file);
    }
}
