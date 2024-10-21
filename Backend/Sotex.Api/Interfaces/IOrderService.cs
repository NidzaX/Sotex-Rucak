using Sotex.Api.Dto.OrderDto;

namespace Sotex.Api.Interfaces
{
    public interface IOrderService
    {
        Task<int> AddOrderAsync(NewOrderDto orderDto);
        Task<bool> CancelOrder(Guid orderId);

    }
}
