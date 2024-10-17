using AutoMapper;
using Sotex.Api.Dto.MenuDto;
using Sotex.Api.Model;

namespace Sotex.Api.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Menu, AddMenuDto>().ReverseMap();
            CreateMap<Dish, DishDto>().ReverseMap();
        }
    }
}

