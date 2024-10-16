using Sotex.Api.Infrastructure;
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

        public Menu AddMenu(Menu memnu)
        {
            Menu retVal = projectDbContext.Menus.Add(memnu).Entity;
            projectDbContext.SaveChanges();
            return retVal;
        }

        public void RemoveMenu(Menu menu)
        {
            projectDbContext.Menus.Remove(menu);
            projectDbContext.SaveChanges();
        }
    }
}
