﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Sotex.Api.Dto.OrderDto;
using Sotex.Api.Infrastructure;
using Sotex.Api.Interfaces;
using Sotex.Api.Model;
using Sotex.Api.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging; 
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
        private readonly ILogger<OrderService> _logger;

        public OrderService(
            IMapper mapper,
            ProjectDbContext dbContext,
            ILogger<OrderService> logger, 
            ILogger<OrdersRepo> ordersRepoLogger)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper), "Mapper cannot be null.");
            _projectDbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext), "Project database context cannot be null.");

            _userRepo = new UserRepo(dbContext);
            _orderRepo = new OrdersRepo(dbContext, ordersRepoLogger);
            _menuRepo = new MenuRepo(dbContext);
            _orderedMenuItemsRepo = new OrderedMenuItemsRepo(dbContext);
        }


        public async Task<Guid> AddOrderAsync(NewOrderDto orderDto)
        {
            if (orderDto == null)
                throw new ArgumentNullException(nameof(orderDto), "Order data cannot be null.");

            var user = await _userRepo.FindUserByUsernameAsync(orderDto.Username);
            if (user == null)
            {
                _logger.LogWarning("User with username {Username} not found.", orderDto.Username);
                throw new Exception("User not found");
            }

            var newOrder = new Order
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                TotalPrice = 0m,
                OrderDate = DateTime.UtcNow,
                ValidUntil = DateTime.UtcNow.AddHours(1),
                IsCancelled = false,
                OrderedMenuItems = new List<OrderedMenuItem>()
            };

            // Check for existing orders with the same Id
            if (await _projectDbContext.Orders.AnyAsync(o => o.Id == newOrder.Id))
            {
                throw new InvalidOperationException("An order with the same ID already exists.");
            }

            // Check if dishes exist in the menu
            var dishIds = orderDto.Dishes.Select(d => d.DishId);
            var sideDishIds = orderDto.SideDishes.Select(sd => sd.SideDishId);
            var dishesExist = await _menuRepo.AreDishesInMenuAsync(dishIds, sideDishIds);
            if (!dishesExist)
            {
                _logger.LogWarning("One or more dishes or side dishes are not available in the menu.");
                throw new InvalidOperationException("One or more dishes or side dishes are not available in the menu.");
            }

            // Process dishes
            foreach (var dishDto in orderDto.Dishes)
            {
                if (dishDto.DishId == Guid.Empty) continue; // Skip if no dish ID is provided

                var dish = await _menuRepo.FindDishByIdAsync(dishDto.DishId);
                if (dish == null)
                    throw new InvalidOperationException("Dish not found in the menu.");

                for (int i = 0; i < dishDto.DishQuantity; i++)
                {
                    var orderedDish = new OrderedMenuItem
                    {
                        OrderedMenuItemId = Guid.NewGuid(), // Ensure unique ID
                        OrderId = newOrder.Id,
                        MenuId = dish.MenuId,
                        DishId = dish.Id,
                        OrderQuantity = 1,
                        MenuItemType = MenuItemType.Dish
                    };

                    // Log the generated ID and check for duplicates
                    _logger?.LogInformation("Generated OrderedMenuItemId for dish: {OrderedMenuItemId}", orderedDish.OrderedMenuItemId);

                    newOrder.TotalPrice += dish.Price;
                    newOrder.OrderedMenuItems.Add(orderedDish);
                }
            }

            // Process side dishes
            foreach (var sideDishDto in orderDto.SideDishes)
            {
                var sideDishEntity = await _menuRepo.FindSideDishByIdAsync(sideDishDto.SideDishId);
                if (sideDishEntity == null)
                    throw new InvalidOperationException("Side dish not found in the menu.");

                for (int i = 0; i < sideDishDto.SideDishQuantity; i++)
                {
                    var orderedSideDish = new OrderedMenuItem
                    {
                        OrderedMenuItemId = Guid.NewGuid(), // Ensure unique ID
                        OrderId = newOrder.Id,
                        MenuId = sideDishEntity.MenuId,
                        SideDishId = sideDishEntity.Id,
                        OrderQuantity = 1,
                        MenuItemType = MenuItemType.SideDish
                    };

                    // Log the generated ID and check for duplicates
                    _logger?.LogInformation("Generated OrderedMenuItemId for side dish: {OrderedMenuItemId}", orderedSideDish.OrderedMenuItemId);

                    newOrder.OrderedMenuItems.Add(orderedSideDish);
                }
            }

            using var transaction = await _projectDbContext.Database.BeginTransactionAsync();
            try
            {
                // Add the order
                _projectDbContext.Orders.Add(newOrder);

                // Add all ordered menu items in a single operation
                foreach (var item in newOrder.OrderedMenuItems)
                {
                    _projectDbContext.OrderedMenuItems.Add(item);
                }

                // Save changes for both orders and ordered menu items
                await _projectDbContext.SaveChangesAsync();

                await transaction.CommitAsync();
                return newOrder.Id;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error while adding order with ID {OrderId}. Rolling back transaction.", newOrder.Id);
                if (ex.InnerException != null)
                {
                    _logger?.LogError("Inner exception: {InnerMessage}", ex.InnerException.Message);
                    _logger?.LogError("Inner exception stack trace: {InnerStackTrace}", ex.InnerException.StackTrace);
                }
                await transaction.RollbackAsync();
                throw; // Rethrow the exception after rollback
            }

        }

        public Task<bool> CancelOrderAsync(Guid orderId)
        {
            throw new NotImplementedException();
        }
    }
}
