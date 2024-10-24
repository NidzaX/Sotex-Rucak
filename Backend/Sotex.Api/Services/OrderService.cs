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
            if (user == null)
            {
                throw new Exception("User not found");
            }

            var firstMenuItem = orderDto.MenuItems.FirstOrDefault();
            if (firstMenuItem == null)
            {
                throw new InvalidOperationException("No menu items provided.");
            }

            var menuId = firstMenuItem.DishId ?? firstMenuItem.SideDishId ?? Guid.Empty;
            Console.WriteLine($"Searching for Menu with ID: {menuId}");
            var menu = await _menuRepo.FindMenuByIdAsync(firstMenuItem.DishId ?? firstMenuItem.SideDishId ?? Guid.Empty);
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
                OrderedMenuItem orderedItem = null;

                // Check if it's a dish
                if (menuItem.DishId.HasValue)
                {
                    var dish = menu.Dishes.FirstOrDefault(d => d.Id == menuItem.DishId);
                    if (dish == null)
                    {
                        throw new InvalidOperationException("Dish not found in the menu.");
                    }

                    orderedItem = new OrderedMenuItem
                    {
                        MenuId = menu.Id,
                        DishId = dish.Id,
                        OrderQuantity = menuItem.OrderQuantity,
                        MenuItemType = MenuItemType.Dish
                    };
                    newOrder.TotalPrice += dish.Price * menuItem.OrderQuantity;
                }
                // Check if it's a side dish
                else if (menuItem.SideDishId.HasValue)
                {
                    var sideDish = menu.SideDishes.FirstOrDefault(sd => sd.Id == menuItem.SideDishId);
                    if (sideDish == null)
                    {
                        throw new InvalidOperationException("Side dish not found in the menu.");
                    }

                    orderedItem = new OrderedMenuItem
                    {
                        MenuId = menu.Id,
                        SideDishId = sideDish.Id,
                        OrderQuantity = menuItem.OrderQuantity,
                        MenuItemType = MenuItemType.SideDish
                    };
                }

                if (orderedItem != null)
                {
                    newOrder.OrderedMenuItems.Add(orderedItem);
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
