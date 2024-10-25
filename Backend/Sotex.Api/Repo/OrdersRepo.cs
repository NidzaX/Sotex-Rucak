using Sotex.Api.Infrastructure;
using Sotex.Api.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Abp.UI;

namespace Sotex.Api.Repo
{
    public class OrdersRepo
    {
        private readonly ProjectDbContext _projectDbContext;
        private readonly ILogger<OrdersRepo> _logger; // Inject logger

        public OrdersRepo(ProjectDbContext context, ILogger<OrdersRepo> logger)
        {
            _projectDbContext = context;
            _logger = logger; // Initialize logger
        }

        public async Task<Order> AddOrderAsync(Order order)
        {
            var retVal =  _projectDbContext.Orders.Add(order);
            await _projectDbContext.SaveChangesAsync();
            return retVal.Entity;
        }

        public async Task<Order> UpdateOrderAsync(Order order)
        {
            const int maxRetryCount = 3;

            for (int retry = 0; retry < maxRetryCount; retry++)
            {
                // Fetch the latest version of the order, including related items
                var existingOrder = await _projectDbContext.Orders
                    .Include(o => o.OrderedMenuItems) // Include related entities
                    .FirstOrDefaultAsync(o => o.Id == order.Id);

                if (existingOrder == null)
                {
                    _logger.LogWarning("Order with ID {OrderId} does not exist or has been deleted.", order.Id);
                    throw new InvalidOperationException("The order does not exist or has been deleted.");
                }

                try
                {
                    // Update scalar properties while preserving the Timestamp
                    _projectDbContext.Entry(existingOrder).CurrentValues.SetValues(order);

                    // Handle related OrderedMenuItems correctly
                    foreach (var item in order.OrderedMenuItems)
                    {
                        var existingItem = existingOrder.OrderedMenuItems
                            .FirstOrDefault(x => x.OrderedMenuItemId == item.OrderedMenuItemId);

                        if (existingItem == null)
                        {
                            // New item: Add it to the collection
                            existingOrder.OrderedMenuItems.Add(item);
                            _projectDbContext.Entry(item).State = EntityState.Added;

                            // Log the addition of a new item
                            _logger.LogInformation("Added new OrderedMenuItemId: {OrderedMenuItemId}", item.OrderedMenuItemId);
                        }
                        else
                        {
                            // Existing item: Update it
                            _projectDbContext.Entry(existingItem).CurrentValues.SetValues(item);
                            _projectDbContext.Entry(existingItem).State = EntityState.Modified;

                            // Log the update of an existing item
                            _logger.LogInformation("Updated OrderedMenuItemId: {OrderedMenuItemId}", existingItem.OrderedMenuItemId);
                        }
                    }

                    // Set the original Timestamp for concurrency control
                    _projectDbContext.Entry(existingOrder).Property(e => e.Timestamp).OriginalValue = order.Timestamp;

                    // Save changes and return the updated order
                    await _projectDbContext.SaveChangesAsync();
                    return existingOrder;
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    _logger.LogError(ex, "Concurrency conflict occurred while updating order with ID {OrderId}.", order.Id);

                    if (retry == maxRetryCount - 1)
                    {
                        throw new UserFriendlyException(ex.Message);
                    }

                    // Log retry attempt
                    _logger.LogInformation("Retrying update for order with ID {OrderId}. Attempt {RetryCount}.", order.Id, retry + 1);
                }
                catch (DbUpdateException ex)
                {
                    _logger.LogError(ex, "Database update error occurred while updating order with ID {OrderId}.", order.Id);
                    throw new InvalidOperationException("An error occurred while updating the order.", ex);
                }
                catch (Exception ex)
                {
                    _logger.LogCritical(ex, "Unexpected error occurred while updating order with ID {OrderId}.", order.Id);
                    throw; // Preserve original exception
                }
            }

            _logger.LogError("Failed to update order with ID {OrderId} after {MaxRetryCount} retries.", order.Id, maxRetryCount);
            throw new InvalidOperationException("Failed to update order after multiple retries.");
        }


        public async Task<List<Order>> GetAllAsync()
        {
            return await _projectDbContext.Orders.ToListAsync();
        }
    }
}
