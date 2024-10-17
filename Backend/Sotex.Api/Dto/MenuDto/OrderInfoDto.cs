using Newtonsoft.Json;

namespace Sotex.Api.Dto.MenuDto
{
    public class OrderInfoDto
    {
        [JsonProperty("phone_number")]
        public string PhoneNumber { get; set; }
        public string Note { get; set; }
    }
}
