namespace Sotex.Api.Interfaces
{
    public interface IOpenAIService
    {
        Task<string> ParseImageFromUrlAsync(string imageUrl, string purpose);
    }
}
