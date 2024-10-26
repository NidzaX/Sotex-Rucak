namespace Sotex.Api.Dto.OrderDto
{
    public class SideDishDto : OrderedMenuItemDto
    {
        public Guid SideDishId { get; set; } 
        public int SideDishQuantity { get; set; } 
    }
}
