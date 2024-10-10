using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sotex.Api.Model;

namespace Sotex.Api.Interfaces.Configurations
{
    public class OrderedMenuItemConfiguration : IEntityTypeConfiguration<OrderedMenuItem>
    {
        public void Configure(EntityTypeBuilder<OrderedMenuItem> builder)
        {
            builder.HasKey(x => new { x.OrderId, x.MenuItemId });
            builder.HasOne(x => x.Order)
                .WithMany(x => x.OrderedMenuItems)
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.MenuItem)
                .WithMany(x => x.OrderedMenuItems)
                .HasForeignKey(x => x.MenuItemId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
