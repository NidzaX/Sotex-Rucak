using Newtonsoft.Json;

namespace Sotex.Api.Dto.MenuDto
{
    public class MenuDetailsDto
    {
        [JsonProperty("dishes")]
        public List<DishDto> Dishes { get; set; }

        [JsonProperty("sides")]
        public List<string> Sides { get; set; }

        [JsonProperty("special_offer")]
        public string SpecialOffer { get; set; }

        [JsonProperty("order_info")] 
        public OrderInfoDto OrderInfo { get; set; } 

    }
}
