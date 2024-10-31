using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph.Models;
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
            if (orderDto == null ||
                (orderDto.Dishes == null && orderDto.SideDishes == null))
            {
                return BadRequest("Order data is required.");
            }

            var userIdClaim = User.FindFirst("id");
            if(userIdClaim == null)
            {
                return Unauthorized(new { error = "User is not authorized" });
            }

            var userId = Guid.Parse(userIdClaim.Value);

            try
            {
                var orderId = await _ordersService.AddOrderAsync(orderDto, userId);
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

        [HttpPost("cancelOrder")]
        public async Task<IActionResult> CancelOrder(Guid orderId)
        {
            var userIdClaim = User.FindFirst("id");
            if(userIdClaim == null)
            {
                return Unauthorized(new { error = "User is not authorized" });
            }

            var userId = Guid.Parse(userIdClaim.Value);

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

        [HttpGet("getUserOrders")]
        public async Task<IActionResult> GetUserOrders()
        {
            var userIdClaim = User.FindFirst("id");
            if (userIdClaim == null)
            {
                return Unauthorized(new { error = "User is not authorized" });
            }

            var userId = Guid.Parse(userIdClaim.Value);
            try
            {
                var orders = await _ordersService.GetAllOrdersAsync(userId);

                if (orders == null || orders.Count == 0)
                {
                    return NotFound("No orders found for the specified user.");
                }
                
                return Ok(orders);
            }catch(Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
