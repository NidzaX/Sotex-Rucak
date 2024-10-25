using Sotex.Api.Model;

namespace Sotex.Api.Dto.OrderDto
{
    public class NewOrderDto
    {
        public string Username { get; set; }
        public List<DishDto> Dishes { get; set; } 
        public List<SideDishDto> SideDishes { get; set; }       
    }
}
