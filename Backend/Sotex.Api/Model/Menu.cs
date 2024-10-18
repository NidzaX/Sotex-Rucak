using System.Diagnostics.Contracts;

namespace Sotex.Api.Model
{
    public class Menu
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string SpecialOffer { get; set; }
        public OrderInfo OrderInfo { get; set; } 
        public Guid UserId { get; set; }
        public User User { get; set; }

        public List<Dish> Dishes { get; set; } = new List<Dish>();
        public List<SideDish> SideDishes { get; set; } = new List<SideDish>();

        public List<OrderedMenuItem> OrderedMenuItems { get; set; } = new List<OrderedMenuItem>();
    }
}