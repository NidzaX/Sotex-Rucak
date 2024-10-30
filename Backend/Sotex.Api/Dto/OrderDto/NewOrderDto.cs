using Sotex.Api.Model;

namespace Sotex.Api.Dto.OrderDto
{
    public class NewOrderDto
    {
        public List<DishDto> Dishes { get; set; } 
        public List<SideDishDto> SideDishes { get; set; }       
    }
}
