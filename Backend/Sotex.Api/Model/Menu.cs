using Microsoft.Graph.Models;
using System.Diagnostics.Contracts;
using System.Text.Json.Serialization;

namespace Sotex.Api.Model
{
    public class Menu
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string SpecialOffer { get; set; }
        public OrderInfo OrderInfo { get; set; } 
        public Guid UserId { get; set; }

        [JsonIgnore]
        public User User { get; set; }

        [JsonIgnore]
        public List<Dish> Dishes { get; set; } = new List<Dish>();

        [JsonIgnore]
        public List<SideDish> SideDishes { get; set; } = new List<SideDish>();

        public List<OrderedMenuItem> OrderedMenuItems { get; set; } = new List<OrderedMenuItem>();
    }
}