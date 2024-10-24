using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Sotex.Api.Model;

public class OrderedMenuItemsConfiguration : IEntityTypeConfiguration<OrderedMenuItem>
{
    public void Configure(EntityTypeBuilder<OrderedMenuItem> builder)
    {
        builder.HasKey(x => new { x.OrderId, x.MenuId, x.DishId, x.SideDishId });

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
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.SideDish)
            .WithMany()
            .HasForeignKey(x => x.SideDishId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
