using Microsoft.Graph.Models;

namespace Sotex.Api.Model
{
    public class Order
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime ValidUntil { get; set; }
        public bool IsCancelled { get; set; }

        public virtual User User { get; set; }
        public virtual List<OrderedMenuItem> OrderedMenuItem { get; set; } = new List<OrderedMenuItem>();

    }
}
