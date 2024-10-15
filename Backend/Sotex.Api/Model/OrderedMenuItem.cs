namespace Sotex.Api.Model
{
    public class OrderedMenuItem
    {
        public Guid OrderId { get; set; }
        public Order Order { get; set; } 
        public Guid MenuId { get; set; }
        public Menu Menu { get; set; }
        public int OrderQuantity { get; set; }

    }
}
