﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Sotex.Api.Infrastructure;

#nullable disable

namespace Sotex.Api.Migrations
{
    [DbContext(typeof(ProjectDbContext))]
    partial class ProjectDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Sotex.Api.Model.Dish", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("MenuId")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.HasIndex("MenuId");

                    b.ToTable("Dishes");
                });

            modelBuilder.Entity("Sotex.Api.Model.Menu", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("SpecialOffer")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Menus");
                });

            modelBuilder.Entity("Sotex.Api.Model.Order", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("IsCancelled")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("OrderDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<decimal>("TotalPrice")
                        .HasColumnType("numeric");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("ValidUntil")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("Sotex.Api.Model.OrderedMenuItem", b =>
                {
                    b.Property<Guid>("OrderId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("MenuId")
                        .HasColumnType("uuid");

                    b.Property<int>("MenuItemType")
                        .HasColumnType("integer");

                    b.Property<Guid?>("DishId")
                        .HasColumnType("uuid");

                    b.Property<int>("OrderQuantity")
                        .HasColumnType("integer");

                    b.Property<Guid?>("SideDishId")
                        .HasColumnType("uuid");

                    b.HasKey("OrderId", "MenuId", "MenuItemType");

                    b.HasIndex("DishId");

                    b.HasIndex("MenuId");

                    b.HasIndex("SideDishId");

                    b.ToTable("OrderedMenuItems");
                });

            modelBuilder.Entity("Sotex.Api.Model.SideDish", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("MenuId")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("MenuId");

                    b.ToTable("SideDishes");
                });

            modelBuilder.Entity("Sotex.Api.Model.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .HasColumnType("text");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Sotex.Api.Model.Dish", b =>
                {
                    b.HasOne("Sotex.Api.Model.Menu", "Menu")
                        .WithMany("Dishes")
                        .HasForeignKey("MenuId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Menu");
                });

            modelBuilder.Entity("Sotex.Api.Model.Menu", b =>
                {
                    b.HasOne("Sotex.Api.Model.User", "User")
                        .WithMany("Menus")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("Sotex.Api.Model.OrderInfo", "OrderInfo", b1 =>
                        {
                            b1.Property<Guid>("MenuId")
                                .HasColumnType("uuid");

                            b1.Property<string>("Note")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("OrderNote");

                            b1.Property<string>("Phone")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("OrderPhone");

                            b1.HasKey("MenuId");

                            b1.ToTable("Menus");

                            b1.WithOwner()
                                .HasForeignKey("MenuId");
                        });

                    b.Navigation("OrderInfo")
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Sotex.Api.Model.Order", b =>
                {
                    b.HasOne("Sotex.Api.Model.User", null)
                        .WithMany("Orders")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Sotex.Api.Model.OrderedMenuItem", b =>
                {
                    b.HasOne("Sotex.Api.Model.Dish", "Dish")
                        .WithMany()
                        .HasForeignKey("DishId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Sotex.Api.Model.Menu", "Menu")
                        .WithMany("OrderedMenuItems")
                        .HasForeignKey("MenuId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Sotex.Api.Model.Order", "Order")
                        .WithMany("OrderedMenuItems")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Sotex.Api.Model.SideDish", "SideDish")
                        .WithMany()
                        .HasForeignKey("SideDishId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Dish");

                    b.Navigation("Menu");

                    b.Navigation("Order");

                    b.Navigation("SideDish");
                });

            modelBuilder.Entity("Sotex.Api.Model.SideDish", b =>
                {
                    b.HasOne("Sotex.Api.Model.Menu", "Menu")
                        .WithMany("SideDishes")
                        .HasForeignKey("MenuId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Menu");
                });

            modelBuilder.Entity("Sotex.Api.Model.Menu", b =>
                {
                    b.Navigation("Dishes");

                    b.Navigation("OrderedMenuItems");

                    b.Navigation("SideDishes");
                });

            modelBuilder.Entity("Sotex.Api.Model.Order", b =>
                {
                    b.Navigation("OrderedMenuItems");
                });

            modelBuilder.Entity("Sotex.Api.Model.User", b =>
                {
                    b.Navigation("Menus");

                    b.Navigation("Orders");
                });
#pragma warning restore 612, 618
        }
    }
}
