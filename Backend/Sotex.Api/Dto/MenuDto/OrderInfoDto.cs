using Newtonsoft.Json;

namespace Sotex.Api.Dto.MenuDto
{
    public class OrderInfoDto
    {
        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("note")]
        public string Note { get; set; }
    }
}
