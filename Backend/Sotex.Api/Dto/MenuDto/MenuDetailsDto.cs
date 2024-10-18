using Newtonsoft.Json;

namespace Sotex.Api.Dto.MenuDto
{
    public class MenuDetailsDto
    {
        public List<DishDto> Dishes { get; set; }  
        public List<string> Sides { get; set; } 
        public string SpecialOffer { get; set; }  
        public string AdditionalInfo { get; set; }

        [JsonProperty("contact")]
        public string ContactInfo { get; set; }  
    }
}
