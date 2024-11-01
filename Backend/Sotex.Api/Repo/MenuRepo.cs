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
                    (m.StartDate <= now && m.EndDate >= now) ||
                    (m.StartDate <= endOfTomorrow && m.EndDate >= tomorrow)
                );
        }

        public async Task<bool> IsMenuActiveAsync(Guid menuId)
        {
            var menu = await _projectDbContext.Menus.FindAsync(menuId);
            if (menu == null)
                throw new InvalidOperationException("Menu not found");
            
            return menu.IsActive;
        }

        public async Task<Menu> FindMenuByIdAsync(Guid menuId)
        {
            return await _projectDbContext.Menus
                .Include(m => m.Dishes)        
                .Include(m => m.SideDishes)    
                .FirstOrDefaultAsync(m => m.Id == menuId);
        }
        public async Task<bool> AreDishesInMenuAsync(IEnumerable<Guid> dishIds, IEnumerable<Guid> sideDishIds)
        {
            // Check for valid dish IDs
            if (dishIds == null || !dishIds.Any() && (sideDishIds == null || !sideDishIds.Any()))
            {
                return true; // No dishes to check, so return true
            }

            // Get the menu items that are valid
            var menuItems = await _projectDbContext.Menus
                .Include(m => m.Dishes)
                .Include(m => m.SideDishes)
                .ToListAsync();

            var dishIdsSet = new HashSet<Guid>(dishIds);
            var sideDishIdsSet = new HashSet<Guid>(sideDishIds);

            // Check if all dishes are in the menu
            foreach (var menu in menuItems)
            {
                if (menu.Dishes.Any(d => dishIdsSet.Contains(d.Id)) || menu.SideDishes.Any(sd => sideDishIdsSet.Contains(sd.Id)))
                {
                    return true; // Found at least one matching dish or side dish
                }
            }

            return false; // No matching dishes or side dishes found in the menus
        }

        public async Task<Dish> FindDishByIdAsync(Guid dishId)
        {
            return await _projectDbContext.Dishes
                .FirstOrDefaultAsync(d => d.Id == dishId);
        }

        public async Task<SideDish> FindSideDishByIdAsync(Guid sideDishId)
        {
            return await _projectDbContext.SideDishes
                .FirstOrDefaultAsync(sd => sd.Id == sideDishId);
        }

        public async Task<List<Menu>> GetAllMenusAsync()
        {
            return await _projectDbContext.Menus
                .Include(x => x.Dishes)
                .Include(x => x.SideDishes)
                .ToListAsync();
        }
    }
}
