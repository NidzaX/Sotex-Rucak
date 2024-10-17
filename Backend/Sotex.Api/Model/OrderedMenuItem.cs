namespace Sotex.Api.Model
{
    public class OrderedMenuItem
    {
        public Guid OrderId { get; set; }
        public Order Order { get; set; } 

        public Guid MenuId { get; set; }
        public Menu Menu { get; set; }

        public Guid? DishId { get; set; }
        public Dish Dish { get; set; }

        public Guid? SideDishId { get; set; }
        public SideDish SideDish { get; set; }

        public MenuItemType MenuItemType { get; set; }
        public int OrderQuantity { get; set; }

    }
}
