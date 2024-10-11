namespace Sotex.Api.Model
{
    public class MenuItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public List<SideDish> SideDishes { get; set; }
        public List<OrderedMenuItem> OrderedMenuItems { get; set; }
    }
}
