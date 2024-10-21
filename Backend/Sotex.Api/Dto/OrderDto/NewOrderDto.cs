using Sotex.Api.Model;

namespace Sotex.Api.Dto.OrderDto
{
    public class NewOrderDto
    {
        public string Username { get; set; }
        public int? Price { get; set; }
        public List<OrderMenuItemQuantityTemp> MenuItems { get; set; }
    }
}
