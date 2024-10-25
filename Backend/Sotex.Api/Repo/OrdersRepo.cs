using Sotex.Api.Infrastructure;
using Sotex.Api.Model;
using Microsoft.EntityFrameworkCore;

namespace Sotex.Api.Repo
{
    public class OrdersRepo
    {
        private readonly ProjectDbContext _projectDbContext;

        public OrdersRepo(ProjectDbContext context)
        {
            _projectDbContext = context;
        }

        public async Task<Order> AddOrderAsync(Order order)
        {
            var retVal = await _projectDbContext.Orders.AddAsync(order);
            await _projectDbContext.SaveChangesAsync();
            return retVal.Entity;
        }

        public async Task<Order> UpdateOrderAsync(Order order)
        {
            try
            {
                var retVal = _projectDbContext.Orders.Update(order);
                await _projectDbContext.SaveChangesAsync();
                return retVal.Entity;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Handle concurrency conflict (e.g., reload entity or inform the user)
                throw new InvalidOperationException("The order was updated or deleted by another process.", ex);
            }
        }


        public async Task<List<Order>> GetAllAsync()
        {
            return await _projectDbContext.Orders.ToListAsync();
        }
    }
}
