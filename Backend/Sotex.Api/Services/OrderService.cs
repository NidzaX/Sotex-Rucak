using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
            // 1. Find the user
            var user = await _userRepo.FindUserByUsernameAsync(orderDto.Username);
            if (user == null)
                throw new Exception("User not found");

            // 2. Create a new order and save it first to get the OrderId
            var newOrder = new Order
            {
                UserId = user.Id,
                TotalPrice = 0m,
                OrderDate = DateTime.UtcNow,
                ValidUntil = DateTime.UtcNow.AddHours(1),
                IsCancelled = false,
                OrderedMenuItems = new List<OrderedMenuItem>()
            };
            await _orderRepo.AddOrderAsync(newOrder);  // Save to get the OrderId

            // 3. Loop through all menu items and process them
            foreach (var menuItem in orderDto.MenuItems)
            {
                var menu = await _menuRepo.FindMenuByDishOrSideDishIdAsync(
                    menuItem.DishId.GetValueOrDefault(),
                    menuItem.SideDishId.GetValueOrDefault());

                if (menu == null)
                    throw new InvalidOperationException("Menu not found.");

                // Handle Dish if present
                if (menuItem.DishId.HasValue)
                {
                    var dish = menu.Dishes.FirstOrDefault(d => d.Id == menuItem.DishId);
                    if (dish == null)
                        throw new InvalidOperationException("Dish not found in the menu.");

                    var orderedDish = new OrderedMenuItem
                    {
                        OrderId = newOrder.Id,
                        MenuId = menu.Id,
                        DishId = dish.Id,
                        OrderQuantity = menuItem.OrderQuantity,
                        MenuItemType = MenuItemType.Dish
                    };

                    newOrder.TotalPrice += dish.Price * menuItem.OrderQuantity;
                    newOrder.OrderedMenuItems.Add(orderedDish);
                }

                // Handle SideDish if present
                if (menuItem.SideDishId.HasValue)
                {
                    var sideDish = menu.SideDishes.FirstOrDefault(sd => sd.Id == menuItem.SideDishId);
                    if (sideDish == null)
                        throw new InvalidOperationException("Side dish not found in the menu.");

                    var orderedSideDish = new OrderedMenuItem
                    {
                        OrderId = newOrder.Id,
                        MenuId = menu.Id,
                        SideDishId = sideDish.Id,
                        OrderQuantity = menuItem.OrderQuantity,
                        MenuItemType = MenuItemType.SideDish
                    };

                    // Add side dish item to the order (assuming side dishes don't contribute to total price)
                    newOrder.OrderedMenuItems.Add(orderedSideDish);
                }
            }

            // 4. Update the order with the ordered items and total price
            await _orderRepo.UpdateOrderAsync(newOrder);

            return newOrder.Id;
        }





        public Task<bool> CancelOrderAsync(Guid orderId)
        {
            throw new NotImplementedException();
        }
    }
}
