using Microsoft.AspNetCore.Mvc;
using Sotex.Api.Interfaces;

namespace Sotex.Api.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _ordersService;
        public OrdersController(IOrderService ordersService)
        {
            _ordersService = ordersService;
        }

        //....
    }
}
