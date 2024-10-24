using Microsoft.EntityFrameworkCore;
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

        public async Task<bool> HasActiveOrUpcomingMenuAsync(Guid userId)
        {
            var now = DateTime.UtcNow;
            var tomorrow = now.AddDays(1).Date;
            var endOfTomorrow = tomorrow.AddHours(23).AddMinutes(59).AddSeconds(59);

            return await _projectDbContext.Menus
                .AnyAsync(m =>
                    m.UserId == userId &&
                    (m.StartDate <= now && m.EndDate >= now) || 
                    (m.StartDate <= endOfTomorrow && m.EndDate >= tomorrow) 
                );
        }

        public async Task<Menu> FindMenuByIdAsync(Guid menuId)
        {
            return await _projectDbContext.Menus
                .Include(m => m.Dishes)        
                .Include(m => m.SideDishes)    
                .FirstOrDefaultAsync(m => m.Id == menuId);
        }
        public async Task<Menu> FindMenuByDishOrSideDishIdAsync(Guid dishId, Guid sideDishId)
        {
            return await _projectDbContext.Menus
                .Include(m => m.Dishes)
                .Include(m => m.SideDishes)
                .Where(m => m.Dishes.Any(d => d.Id == dishId) || m.SideDishes.Any(sd => sd.Id == sideDishId))
                .FirstOrDefaultAsync();
        }
    }
}
