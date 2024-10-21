using Sotex.Api.Dto.OrderDto;
using Sotex.Api.Interfaces;

namespace Sotex.Api.Services
{
    public class OrderService : IOrderService
    {
        public Task<int> AddOrderAsync(NewOrderDto orderDto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CancelOrder(Guid orderId)
        {
            throw new NotImplementedException();
        }
    }
}
