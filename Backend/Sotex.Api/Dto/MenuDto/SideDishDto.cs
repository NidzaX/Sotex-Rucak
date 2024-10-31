using Newtonsoft.Json;

namespace Sotex.Api.Dto.MenuDto
{
    public class SideDishDto
    {
        public Guid SideDishId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
