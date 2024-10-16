using Sotex.Api.Dto.OrderDto;

namespace Sotex.Api.Interfaces
{
    public interface IOrderService
    {
        int AddOrder(NewOrderDto orderDto);
        bool CancelOrder(Guid orderId);

    }
}
