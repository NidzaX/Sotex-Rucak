using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sotex.Api.Model;

namespace Sotex.Api.Interfaces.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.HasMany(x => x.OrderedMenuItems)
                   .WithOne()
                   .HasForeignKey(x => x.OrderId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
