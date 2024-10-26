using Sotex.Api.Model;

namespace Sotex.Api.Dto.OrderDto
{
    public class OrderedMenuItemDto
    {
        //public Guid OrderedMenuItemId { get; set; }
        //public Guid OrderId { get; set; }
        //public Guid? MenuId { get; set; }
        //public Guid? DishId { get; set; }
        //public Guid? SideDishId { get; set; }
        //public int OrderQuantity { get; set; }
        public string DishName { get; set; }
        public string SideDishName { get; set; }
        public string MenuItemType { get; set; }
        

    }
}
