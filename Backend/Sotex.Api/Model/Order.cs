namespace Sotex.Api.Model
{
    public class Order
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public int TotalPrice { get; set; }
        public List<OrderedMenuItem> OrderedMenuItems { get; set; } 
        public bool IsCancelled { get; set; } = false;  
    }
}
