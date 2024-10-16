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
            builder.HasKey(x => new { x.OrderId, x.MenuId });
            builder.HasOne(x => x.Order)
                .WithMany(x => x.OrderedMenuItem)
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Menu)
                .WithMany(x => x.OrderedMenuItems)
                .HasForeignKey(x => x.MenuId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
