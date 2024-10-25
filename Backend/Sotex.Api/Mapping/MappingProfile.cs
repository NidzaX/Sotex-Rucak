using AutoMapper;
using Sotex.Api.Dto.MenuDto;
using Sotex.Api.Dto.OrderDto;
using Sotex.Api.Dto.UserDto;
using Sotex.Api.Model;

namespace Sotex.Api.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Map from Menu to AddMenuDto
            CreateMap<Menu, AddMenuDto>()
                .ForMember(dest => dest.Day, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Dishes, opt => opt.MapFrom(src => src.Dishes ?? new List<Dish>())) // Handle null Dishes
                .ForMember(dest => dest.Sides, opt => opt.MapFrom(src => src.SideDishes != null
                    ? src.SideDishes.Select(sd => sd.Name).ToList()
                    : new List<string>())) // Handle null SideDishes
                .ForMember(dest => dest.SpecialOffer, opt => opt.MapFrom(src => src.SpecialOffer))
                .ForMember(dest => dest.OrderInfo, opt => opt.MapFrom(src => src.OrderInfo != null
                    ? new OrderInfoDto
                    {
                        Phone = src.OrderInfo.Phone,
                        Note = src.OrderInfo.Note
                    }
                    : new OrderInfoDto())) // Handle null OrderInfo
                .ReverseMap();

            // Map Dish to DishDto and vice versa
            CreateMap<Dish, Sotex.Api.Dto.MenuDto.DishDto>().ReverseMap();

            CreateMap<User, GoogleRegisterDto>().ReverseMap();
            CreateMap<Order, GetAllOrdersDto>().ReverseMap();
        }
    }
}
