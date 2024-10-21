using AutoMapper;
using Sotex.Api.Dto.OrderDto;
using Sotex.Api.Infrastructure;
using Sotex.Api.Interfaces;
using Sotex.Api.Model;
using Sotex.Api.Repo;

namespace Sotex.Api.Services
{
    public class OrderService : IOrderService
    {
        private readonly IMapper _mapper;
        private readonly ProjectDbContext _projectDbContext;
        private readonly UserRepo _userRepo;
        private readonly OrdersRepo _orderRepo;
        private readonly MenuRepo _menuRepo;
        private readonly OrderedMenuItemsRepo _orderedMenuItemsRepo;

        public OrderService(IMapper mapper, ProjectDbContext dbContext)
        {
            _mapper = mapper;
            _userRepo = new UserRepo(dbContext);
            _orderRepo = new OrdersRepo(dbContext);
            _menuRepo = new MenuRepo(dbContext);
            _orderedMenuItemsRepo = new OrderedMenuItemsRepo(dbContext);
        }
        public async Task<Guid> AddOrderAsync(NewOrderDto orderDto)
        {
            var user = await _userRepo.FindUserByUsernameAsync(orderDto.Username);
            if(user == null)
            {
                throw new Exception("User not found");
            }

            var menuId = orderDto.MenuItems.First().MenuId;
            var menu = await _menuRepo.FindMenuByIdAsync(menuId);

            if (menu == null)
            {
                throw new InvalidOperationException("Menu not found.");
            }

            var newOrder = new Order
            {
                UserId = user.Id,
                TotalPrice = 0m,
                OrderDate = DateTime.UtcNow,
                ValidUntil = DateTime.UtcNow.AddHours(1),
                IsCancelled = false,
                OrderedMenuItems = new List<OrderedMenuItem>()
            };

            foreach (var menuItem in orderDto.MenuItems)
            {
                var dish = menu.Dishes.FirstOrDefault(d => d.Id == menuItem.MenuId);
                var sideDish = menu.SideDishes.FirstOrDefault(sd => sd.Id == menuItem.MenuId);

                if(dish != null)
                {
                    var orderedItem = new OrderedMenuItem
                    {
                        MenuId = menu.Id,
                        DishId = dish.Id,
                        OrderQuantity = menuItem.OrderQuantity,
                        MenuItemType = MenuItemType.Dish
                    };

                    newOrder.OrderedMenuItems.Add(orderedItem);
                    newOrder.TotalPrice += dish.Price * menuItem.OrderQuantity;
                }
                else if(sideDish != null)
                {
                    var orderedItem = new OrderedMenuItem
                    {
                        MenuId = menu.Id,
                        SideDishId = sideDish.Id,
                        OrderQuantity = menuItem.OrderQuantity,
                        MenuItemType = MenuItemType.SideDish
                    };

                    newOrder.OrderedMenuItems.Add(orderedItem);
                }
                else
                {
                    throw new InvalidOperationException("Menu item not found in the menu.");
                }
            }

            await _orderRepo.AddOrderAsync(newOrder);

            return newOrder.Id;
        }

        public Task<bool> CancelOrderAsync(Guid orderId)
        {
            throw new NotImplementedException();
        }
    }
}
