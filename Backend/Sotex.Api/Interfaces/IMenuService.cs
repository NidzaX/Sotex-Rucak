using Microsoft.AspNetCore.Mvc;

namespace Sotex.Api.Interfaces
{
    public interface IMenuService
    {
        Task<string> ParseImageFromFileAsync(IFormFile file, string purpose);

    }
}
