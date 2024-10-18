using Microsoft.AspNetCore.Mvc;
using Sotex.Api.Model;

namespace Sotex.Api.Interfaces
{
    public interface IMenuService
    {
       Task<Menu> ParseAndSaveMenuFromFileAsync(IFormFile file, string purpose, Guid userId);

    }
}
