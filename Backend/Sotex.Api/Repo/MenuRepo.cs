using Sotex.Api.Interfaces;
using Sotex.Api.Model;

namespace Sotex.Api.Repo
{
    public class MenuRepo
    {
        private ProjectDbContext projectDbContext;

        public MenuRepo(ProjectDbContext context)
        {
            projectDbContext = context;
        }

        public Menu AddMenu(Menu m)
        {
            Menu retVal = projectDbContext.Menus.Add(m).Entity;
            projectDbContext.SaveChanges();
            return retVal;
        }

        public void RemoveMenu(Menu m)
        {
            projectDbContext.Menus.Remove(m);
            projectDbContext.SaveChanges();
        }
    }
}
