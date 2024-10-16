using Sotex.Api.Infrastructure;
using Sotex.Api.Model;

namespace Sotex.Api.Repo
{
    public class OrdersRepo
    {
        private ProjectDbContext projectDbContext;

        public OrdersRepo(ProjectDbContext context)
        {
            projectDbContext = context;
        }

        public Order AddOrder(Order order)
        {
            Order retVal = projectDbContext.Orders.Add(order).Entity;
            projectDbContext.SaveChanges();
            return retVal;
        }

        public Order UpdateOrder(Order order)
        {
            Order retVal = projectDbContext.Orders.Update(order).Entity;
            projectDbContext.SaveChanges();
            return retVal;
        }

        public List<Order> GetAll()
        {
            return projectDbContext.Orders.ToList();
        }
    }
}
