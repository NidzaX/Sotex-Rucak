using Microsoft.Graph.Drives.Item.Items.Item.Workbook.Names.Item.RangeNamespace.ColumnsBeforeWithCount;
using Sotex.Api.Model;

namespace Sotex.Api.Dto.MenuDto
{
    public class AddMenuDto
    {
        public string Name { get; set; }
        public decimal? Price { get; set; }

        public List<AddDishDto> Dishes { get; set; } = new List<AddDishDto>();
        public List<string> SideDishes { get; set; } = new List<string>();
    }
}
