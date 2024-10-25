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

            // 2. Create a new order
            var newOrder = new Order
            {
                UserId = user.Id,
                TotalPrice = 0m,
                OrderDate = DateTime.UtcNow,
                ValidUntil = DateTime.UtcNow.AddHours(1),
                IsCancelled = false,
                OrderedMenuItems = new List<OrderedMenuItem>()
            };
            await _orderRepo.AddOrderAsync(newOrder);

            // 3. Check if the dishes and side dishes are in the menu
            var dishIds = orderDto.Dishes.Select(d => d.DishId);
            var sideDishIds = orderDto.SideDishes.Select(sd => sd.SideDishId);

            var dishesExist = await _menuRepo.AreDishesInMenuAsync(dishIds, sideDishIds);
            if (!dishesExist)
                throw new InvalidOperationException("One or more dishes or side dishes are not available in the menu.");

            // 4. Process each dish in the order
            foreach (var dishDto in orderDto.Dishes)
            {
                if (dishDto.DishId == Guid.Empty)
                    continue; // Skip if no dish ID is provided

                var dish = await _menuRepo.FindDishByIdAsync(dishDto.DishId);
                if (dish == null)
                    throw new InvalidOperationException("Dish not found in the menu.");

                var orderedDish = new OrderedMenuItem
                {
                    OrderedMenuItemId = Guid.NewGuid(),
                    OrderId = newOrder.Id,
                    MenuId = dish.MenuId,
                    DishId = dish.Id,
                    OrderQuantity = dishDto.DishQuantity,
                    MenuItemType = MenuItemType.Dish
                };

                newOrder.TotalPrice += dish.Price * dishDto.DishQuantity;
                newOrder.OrderedMenuItems.Add(orderedDish);
            }

            // 5. Process each side dish in the order
            foreach (var sideDishDto in orderDto.SideDishes)
            {
                var sideDishEntity = await _menuRepo.FindSideDishByIdAsync(sideDishDto.SideDishId);
                if (sideDishEntity == null)
                    throw new InvalidOperationException("Side dish not found in the menu.");

                var orderedSideDish = new OrderedMenuItem
                {
                    OrderedMenuItemId = Guid.NewGuid(),
                    OrderId = newOrder.Id,
                    MenuId = sideDishEntity.MenuId,
                    SideDishId = sideDishEntity.Id,
                    OrderQuantity = sideDishDto.SideDishQuantity,
                    MenuItemType = MenuItemType.SideDish
                };

                newOrder.OrderedMenuItems.Add(orderedSideDish);
            }

            // 6. Save the new order to the database
            await _orderRepo.UpdateOrderAsync(newOrder);

            return newOrder.Id; // Return the new order ID
        }

        public Task<bool> CancelOrderAsync(Guid orderId)
        {
            throw new NotImplementedException();
        }
    }
}
