using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sotex.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddedDishAndSideDishTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Menus");

            migrationBuilder.AddColumn<Guid>(
                name: "DishId",
                table: "OrderedMenuItems",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MenuItemType",
                table: "OrderedMenuItems",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "SideDishId",
                table: "OrderedMenuItems",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Dishes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    MenuId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dishes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Dishes_Menus_MenuId",
                        column: x => x.MenuId,
                        principalTable: "Menus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SideDishes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    MenuId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SideDishes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SideDishes_Menus_MenuId",
                        column: x => x.MenuId,
                        principalTable: "Menus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderedMenuItems_DishId",
                table: "OrderedMenuItems",
                column: "DishId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderedMenuItems_SideDishId",
                table: "OrderedMenuItems",
                column: "SideDishId");

            migrationBuilder.CreateIndex(
                name: "IX_Dishes_MenuId",
                table: "Dishes",
                column: "MenuId");

            migrationBuilder.CreateIndex(
                name: "IX_SideDishes_MenuId",
                table: "SideDishes",
                column: "MenuId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderedMenuItems_Dishes_DishId",
                table: "OrderedMenuItems",
                column: "DishId",
                principalTable: "Dishes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderedMenuItems_SideDishes_SideDishId",
                table: "OrderedMenuItems",
                column: "SideDishId",
                principalTable: "SideDishes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderedMenuItems_Dishes_DishId",
                table: "OrderedMenuItems");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderedMenuItems_SideDishes_SideDishId",
                table: "OrderedMenuItems");

            migrationBuilder.DropTable(
                name: "Dishes");

            migrationBuilder.DropTable(
                name: "SideDishes");

            migrationBuilder.DropIndex(
                name: "IX_OrderedMenuItems_DishId",
                table: "OrderedMenuItems");

            migrationBuilder.DropIndex(
                name: "IX_OrderedMenuItems_SideDishId",
                table: "OrderedMenuItems");

            migrationBuilder.DropColumn(
                name: "DishId",
                table: "OrderedMenuItems");

            migrationBuilder.DropColumn(
                name: "MenuItemType",
                table: "OrderedMenuItems");

            migrationBuilder.DropColumn(
                name: "SideDishId",
                table: "OrderedMenuItems");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Orders",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Menus",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
