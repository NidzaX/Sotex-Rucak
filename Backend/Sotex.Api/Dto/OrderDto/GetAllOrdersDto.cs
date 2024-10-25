using Sotex.Api.Model;

namespace Sotex.Api.Dto.OrderDto
{
    public class GetAllOrdersDto
    {
        public Guid Id { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime ValidUntil { get; set; }
        public bool IsCancelled { get; set; }
        public List<OrderedMenuItem> OrderedMenuItems { get; set; } = new List<OrderedMenuItem>();

    }
}
