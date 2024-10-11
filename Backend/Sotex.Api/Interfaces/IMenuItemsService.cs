using Sotex.Api.Dto;
using Sotex.Api.Model;

namespace Sotex.Api.Interfaces
{
    public interface IMenuItemsService
    {
        MenuItem AddMenuItem(AddMenuItemDto dto);
        List<GetMenuItemsDto> GetAllProducts();        
    }
}
