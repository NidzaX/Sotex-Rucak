using Microsoft.AspNetCore.Mvc;
using Sotex.Api.Interfaces;

namespace Sotex.Api.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        //users endpoints ....
    }
}
