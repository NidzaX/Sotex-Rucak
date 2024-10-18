namespace Sotex.Api.Dto.MenuDto
{
    public class MenuDetailsDto
    {
        public List<DishDto> Dishes { get; set; }  
        public List<string> Sides { get; set; } 
        public string SpecialOffer { get; set; }  
        public string AdditionalInfo { get; set; } 
        public OrderInfoDto ContactInfo { get; set; }  
    }
}
