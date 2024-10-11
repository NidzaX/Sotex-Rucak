using Sotex.Api.Model;

namespace Sotex.Api.Dto
{
    public class GetMenuItemsDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
        public List<SideDish> sideDishes { get; set; }

    }
}
