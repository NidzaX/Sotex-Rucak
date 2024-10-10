using Microsoft.AspNetCore.Mvc;
using Sotex.Api.Interfaces;

namespace Sotex.Api.Controllers
{
    [Route("api/MenuItems")]
    [ApiController]
    public class MenuItemsController : ControllerBase
    {
        private readonly IMenuItemsService _menuItemsService;

        public MenuItemsController(IMenuItemsService menuItemsService)
        {
            _menuItemsService = menuItemsService;
        }

        //....menuItems endpoints here
    }
}
