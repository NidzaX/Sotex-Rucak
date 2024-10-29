using Newtonsoft.Json;

namespace Sotex.Api.Dto.MenuDto
{
    public class SideDishDto
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
