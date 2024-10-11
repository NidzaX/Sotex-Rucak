using Sotex.Api.Model;

namespace Sotex.Api.Dto
{
    public class AddMenuItemDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public List<SideDish> sideDishes { get; set; }

    }
}
