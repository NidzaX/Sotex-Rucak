using Newtonsoft.Json;

namespace Sotex.Api.Dto.MenuDto
{
    public class DishDto
    {
        [JsonProperty("dish")]
        public string Name { get; set; }  

        public string Price { get; set; }  
    }
}
