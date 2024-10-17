using Sotex.Api.Infrastructure;
using Sotex.Api.Model;
using System.Threading.Tasks;

namespace Sotex.Api.Repo
{
    public class MenuRepo
    {
        private readonly ProjectDbContext _projectDbContext;

        public MenuRepo(ProjectDbContext context)
        {
            _projectDbContext = context;
        }

        public async Task<Menu> AddMenuAsync(Menu menu)
        {
            var entityEntry = await _projectDbContext.Menus.AddAsync(menu);
            await _projectDbContext.SaveChangesAsync();
            return entityEntry.Entity;
        }

        public void RemoveMenu(Menu menu)
        {
            _projectDbContext.Menus.Remove(menu);
            _projectDbContext.SaveChanges();
        }
    }
}
