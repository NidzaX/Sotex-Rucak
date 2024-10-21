using System.Text.Json.Serialization;

namespace Sotex.Api.Model
{
    public class Dish
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        public Guid MenuId { get; set; }

        [JsonIgnore]
        public Menu Menu { get; set; }
    }
}
