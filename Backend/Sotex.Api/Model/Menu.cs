using Microsoft.Graph.Drives.Item.Items.Item.GetActivitiesByIntervalWithStartDateTimeWithEndDateTimeWithInterval;
using Microsoft.Graph.Models;
using System.Diagnostics.Contracts;
using System.Text.Json.Serialization;

namespace Sotex.Api.Model
{
    public class Menu
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        private bool _isActive;
        private bool _isActiveTomorrow;
        public bool IsActive
        {
            get => DateTime.UtcNow >=StartDate && DateTime.UtcNow <= EndDate;
            set => _isActive = value;
        }
        public bool IsActiveTomorrow
        {
            get => DateTime.UtcNow.AddDays(1).Date >= StartDate && DateTime.UtcNow.AddDays(1) <= EndDate;
            set => _isActiveTomorrow = value;
        }
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