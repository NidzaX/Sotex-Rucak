using Newtonsoft.Json;

namespace Sotex.Api.Dto.MenuDto
{
    public class DishDto
    {
        public Guid DishId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("price")]
        public string Price { get; set; }
    }
}
