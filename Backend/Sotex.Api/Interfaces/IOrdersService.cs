using Sotex.Api.Dto;

namespace Sotex.Api.Interfaces
{
    public interface IOrdersService
    {
        int AddOrder(NewOrderMenuItemDto dto);
        List<GetAllOrdersDto> GetAllOrders();
        bool CancelOrder(long orderId);
        public List<GetOrderDetailsMenuItemDto> GetOrderDetails(long orderId);
    }
}
