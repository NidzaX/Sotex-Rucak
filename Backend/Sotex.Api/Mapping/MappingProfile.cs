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
                .ReverseMap();

            CreateMap<MenuDetailsDto, Menu>()
                .ForMember(dest => dest.SideDishes,
                           opt => opt.MapFrom(src => src.Sides.Select(s => new SideDish { Name = s })))
                .ForMember(dest => dest.Dishes,
                           opt => opt.MapFrom(src => src.Dishes))
                .ForMember(dest => dest.SpecialOffer,
                           opt => opt.MapFrom(src => src.SpecialOffer))
                .ForMember(dest => dest.AdditionalInfo,
                           opt => opt.MapFrom(src => src.AdditionalInfo))
                .ForMember(dest => dest.ContactInfo,
                           opt => opt.MapFrom(src => src.ContactInfo)) 
                .ReverseMap();

            CreateMap<Dish, DishDto>().ReverseMap();
        }
    }
}

