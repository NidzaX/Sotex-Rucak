using Sotex.Api.Dto.OrderDto;

namespace Sotex.Api.Interfaces
{
    public interface IOrderService
    {
        Task<Guid> AddOrderAsync(NewOrderDto orderDto, Guid userId);
        Task<bool> CancelOrderAsync(Guid orderId);
        public Task<List<GetAllOrdersDto>> GetAllOrdersAsync(Guid userId);

    }
}
