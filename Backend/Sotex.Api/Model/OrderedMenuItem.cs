namespace Sotex.Api.Model
{
    public class OrderedMenuItem
    {
        public Guid OrderId { get; set; }  
        public Order Order { get; set; }
        public Guid MenuItemId { get; set; }  
        public MenuItem MenuItem { get; set; }
        public int OrderQuantity { get; set; }  
    }
}
