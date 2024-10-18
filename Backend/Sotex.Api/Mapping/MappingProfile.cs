using AutoMapper;
using Sotex.Api.Dto.MenuDto;
using Sotex.Api.Model;

namespace Sotex.Api.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Menu, AddMenuDto>()
                .ForMember(dest => dest.Day, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Dishes, opt => opt.MapFrom(src => src.Dishes))
                .ForMember(dest => dest.Sides, opt => opt.MapFrom(src => src.SideDishes.Select(sd => sd.Name)))
                .ForMember(dest => dest.SpecialOffer, opt => opt.MapFrom(src => src.SpecialOffer))
                .ForMember(dest => dest.OrderInfo, opt => opt.MapFrom(src => new OrderInfoDto
                {
                    Phone = src.OrderInfo.Phone,
                    Note = src.OrderInfo.Note
                }))
                .ReverseMap();

            CreateMap<Dish, DishDto>().ReverseMap();
        }
    }
}

