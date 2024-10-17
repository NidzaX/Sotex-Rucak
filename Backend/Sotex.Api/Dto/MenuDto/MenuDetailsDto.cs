namespace Sotex.Api.Dto.MenuDto
{
    public class MenuDetailsDto
    {
        public List<DishDto> Dishes { get; set; }
        public List<string> Sides { get; set; }
        public OrderInfoDto OrderInfo { get; set; }
    }
}
