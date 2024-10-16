using Sotex.Api.Infrastructure;
using Sotex.Api.Model;

namespace Sotex.Api.Repo
{
    public class OrderedMenuItemsRepo
    {
        private ProjectDbContext projectDbContext;
        public OrderedMenuItemsRepo(ProjectDbContext context)
        {
            projectDbContext = context;
        }
        public OrderedMenuItem AddOrderedMenuItem(OrderedMenuItem orderedMenuItem)
        {
            OrderedMenuItem retVal = projectDbContext.OrderedMenuItems.Add(orderedMenuItem).Entity;
            projectDbContext.SaveChanges();
            return retVal;
        }
        public List<OrderedMenuItem> GetAll()
        {
            return projectDbContext.OrderedMenuItems.ToList<OrderedMenuItem>();
        }
    }
}
