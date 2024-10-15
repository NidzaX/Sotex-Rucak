using System.Diagnostics.Contracts;

namespace Sotex.Api.Model
{
    public class Menu
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal? Price { get; set; }
        public MenuItemType Type { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

        public List<OrderedMenuItem> OrderedMenuItems { get; set; } = new List<OrderedMenuItem>();
    }
}
