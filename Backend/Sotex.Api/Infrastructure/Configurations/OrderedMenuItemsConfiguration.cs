using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sotex.Api.Model;

namespace Sotex.Api.Infrastructure.Configurations
{
    public class OrderedMenuItemsConfiguration : IEntityTypeConfiguration<OrderedMenuItem>
    {
        public void Configure(EntityTypeBuilder<OrderedMenuItem> builder)
        {
            builder.HasKey(x => new { x.OrderId, x.MenuId, x.MenuItemType });
            builder.HasOne(x => x.Order)
                .WithMany(x => x.OrderedMenuItems)
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Menu)
                .WithMany(x => x.OrderedMenuItems)
                .HasForeignKey(x => x.MenuId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Dish)
                .WithMany()
                .HasForeignKey(x => x.DishId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.SideDish)
                .WithMany()
                .HasForeignKey(x => x.SideDishId)
                .IsRequired(false) 
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
