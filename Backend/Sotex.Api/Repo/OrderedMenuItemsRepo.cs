using Sotex.Api.Infrastructure;
using Sotex.Api.Model;
using Microsoft.EntityFrameworkCore;

namespace Sotex.Api.Repo
{
    public class OrderedMenuItemsRepo
    {
        private readonly ProjectDbContext _projectDbContext;

        public OrderedMenuItemsRepo(ProjectDbContext context)
        {
            _projectDbContext = context;
        }

        public async Task<OrderedMenuItem> AddOrderedMenuItemAsync(OrderedMenuItem orderedMenuItem)
        {
            var retVal = await _projectDbContext.OrderedMenuItems.AddAsync(orderedMenuItem);
            await _projectDbContext.SaveChangesAsync();
            return retVal.Entity;
        }

        public async Task<List<OrderedMenuItem>> GetAllAsync()
        {
            return await _projectDbContext.OrderedMenuItems.ToListAsync();
        }
    }
}
