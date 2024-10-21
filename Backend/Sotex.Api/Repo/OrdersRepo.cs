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
            var retVal = _projectDbContext.Orders.Update(order);
            await _projectDbContext.SaveChangesAsync();
            return retVal.Entity;
        }

        public async Task<List<Order>> GetAllAsync()
        {
            return await _projectDbContext.Orders.ToListAsync();
        }
    }
}
