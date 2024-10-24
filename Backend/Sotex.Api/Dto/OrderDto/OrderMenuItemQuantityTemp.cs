namespace Sotex.Api.Dto.OrderDto
{
    public class OrderMenuItemQuantityTemp
    {
        public Guid? DishId { get; set; }
        public Guid? SideDishId { get; set; }
        public int OrderQuantity {  get; set; }
    }
}
