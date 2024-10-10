using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Sotex.Api.Model;

namespace Sotex.Api.Interfaces.Configurations
{
    public class SideDishConfiguration : IEntityTypeConfiguration<SideDish>
    {
        public void Configure(EntityTypeBuilder<SideDish> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.HasOne(x => x.MenuItem)
                .WithMany(x => x.sideDishes)
                .HasForeignKey(x => x.MenuItemId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
