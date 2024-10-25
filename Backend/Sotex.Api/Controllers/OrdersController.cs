using Microsoft.AspNetCore.Mvc;
using Sotex.Api.Dto.OrderDto;
using Sotex.Api.Interfaces;
using Sotex.Api.Services;
using System.Reflection.Metadata;

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
        public async Task<IActionResult> AddOrder([FromBody] NewOrderDto orderDto)
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

        [HttpPost("cancelOrder/{orderId}")]
        public async Task<IActionResult> CancelOrder(Guid orderId)
        {
            try
            {
                var result = await _ordersService.CancelOrderAsync(orderId);
                if(result)
                {
                    return Ok(new { Message = "Ordered cancelled successfully" });
                }

                return BadRequest(new { Message = "Order is alredy cancelled" });

            }catch(InvalidOperationException ex)
            {
                return NotFound(new { Error = ex.Message });

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
