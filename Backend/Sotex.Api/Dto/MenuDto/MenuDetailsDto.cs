using Newtonsoft.Json;

namespace Sotex.Api.Dto.MenuDto
{
    public class MenuDetailsDto
    {
        public List<DishDto> Dishes { get; set; }  
        public List<string> Sides { get; set; } 
        public string SpecialOffer { get; set; }

        [JsonProperty("order_info")] 
        public OrderInfoDto OrderInfo { get; set; } 

    }
}
