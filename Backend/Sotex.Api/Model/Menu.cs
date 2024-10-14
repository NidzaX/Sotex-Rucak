using System.Diagnostics.Contracts;

namespace Sotex.Api.Model
{
    public class Menu
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public string? OrderInfo { get; set; }
        public string? CustomerMessage { get; set; } 
        public string? Notes { get; set; }
        public Day? MenuDay { get; set; }
        public MenuItemType? Type { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
