using Sotex.Api.Dto.OrderDto;

namespace Sotex.Api.Interfaces
{
    public interface IOrderService
    {
        Task<Guid> AddOrderAsync(NewOrderDto orderDto);
        Task<bool> CancelOrderAsync(Guid orderId);
        public Task<List<GetAllOrdersDto>> GetAllOrdersAsync(Guid userId);

    }
}
