using Newtonsoft.Json;

namespace Sotex.Api.Dto.MenuDto
{
    public class MenuDto
    {
        [JsonProperty("dishes")]
        public List<DishDto> Dishes { get; set; } = new List<DishDto>();

        [JsonProperty("sideDishes")]
        public List<SideDishDto> SideDishes { get; set; } = new List<SideDishDto>();
    }
}
