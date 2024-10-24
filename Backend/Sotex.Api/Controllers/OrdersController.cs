using Microsoft.AspNetCore.Mvc;
using Sotex.Api.Dto.OrderDto;
using Sotex.Api.Interfaces;
using Sotex.Api.Services;

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

        [HttpPost("addOrder")]
        public async Task<IActionResult> AddOrder([FromForm] NewOrderDto orderDto)
        {
            if (orderDto == null)
            {
                return BadRequest("Order data is required.");
            }

            try
            {
                var orderId = await _ordersService.AddOrderAsync(orderDto);
                return Ok(new { OrderId = orderId });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
