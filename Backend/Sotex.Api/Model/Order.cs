using Microsoft.Graph.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Sotex.Api.Model
{
    public class Order
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime ValidUntil { get; set; }
        public bool IsCancelled { get; set; }

        [Timestamp]
        public byte[] Timestamp { get; set; } // EF Core will use this for concurrency checking

        [JsonIgnore]
        public List<OrderedMenuItem> OrderedMenuItems { get; set; } = new List<OrderedMenuItem>();

    }
}
