using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Sotex.Api.Model;

namespace Sotex.Api.Infrastructure
{
    public class ProjectDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<SideDish> SideDishes { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderedMenuItem> OrderedMenuItems { get; set; }
        public ProjectDbContext(DbContextOptions options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProjectDbContext).Assembly);
        }
    }
}
