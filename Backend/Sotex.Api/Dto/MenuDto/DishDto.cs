using Newtonsoft.Json;

namespace Sotex.Api.Dto.MenuDto
{
    public class DishDto
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("price")]
        public string Price { get; set; }  
    }
}
