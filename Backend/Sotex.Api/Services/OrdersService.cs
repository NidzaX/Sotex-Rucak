using Sotex.Api.Dto;
using Sotex.Api.Interfaces;

namespace Sotex.Api.Services
{
    public class OrdersService : IOrdersService
    {
        public int AddOrder(NewOrderMenuItemDto dto)
        {
            throw new NotImplementedException();
        }

        public bool CancelOrder(long orderId)
        {
            throw new NotImplementedException();
        }

        public List<GetAllOrdersDto> GetAllOrders()
        {
            throw new NotImplementedException();
        }

        public List<GetOrderDetailsMenuItemDto> GetOrderDetails(long orderId)
        {
            throw new NotImplementedException();
        }
    }
}
