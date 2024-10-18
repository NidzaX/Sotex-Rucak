using Microsoft.Graph.Drives.Item.Items.Item.Workbook.Names.Item.RangeNamespace.ColumnsBeforeWithCount;
using Newtonsoft.Json;
using Sotex.Api.Model;

namespace Sotex.Api.Dto.MenuDto
{
    public class AddMenuDto
    {
        public string Day { get; set; }

        [JsonProperty("dishes")]
        public List<DishDto> Dishes { get; set; }

        [JsonProperty("side_dishes")]
        public List<string> Sides { get; set; }

        [JsonProperty("special_offer")]
        public string SpecialOffer { get; set; }

        [JsonProperty("order_info")]
        public OrderInfoDto OrderInfo { get; set; }
    }
}
