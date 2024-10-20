﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sotex.Api.Model;

namespace Sotex.Api.Infrastructure.Configurations
{
    public class MenuConfiguration : IEntityTypeConfiguration<Menu>
    {
        public void Configure(EntityTypeBuilder<Menu> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.HasOne(x => x.User)
                .WithMany(x => x.Menus)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.OrderedMenuItems)
                .WithOne(x => x.Menu)
                .HasForeignKey(x => x.MenuId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.Dishes)
                .WithOne(x => x.Menu)
                .HasForeignKey(x => x.MenuId);

            builder.HasMany(x => x.SideDishes)
                .WithOne(x => x.Menu)
                .HasForeignKey(x => x.MenuId);

            builder.OwnsOne(x => x.OrderInfo, oi =>
            {
                oi.Property(o => o.Phone).HasColumnName("OrderPhone");
                oi.Property(o => o.Note).HasColumnName("OrderNote");
            });
        }
    }
}
