using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sotex.Api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOrderedMenuItemPrimaryKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderedMenuItems",
                table: "OrderedMenuItems");

            migrationBuilder.AddColumn<Guid>(
                name: "OrderedMenuItemId",
                table: "OrderedMenuItems",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderedMenuItems",
                table: "OrderedMenuItems",
                column: "OrderedMenuItemId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderedMenuItems_OrderId",
                table: "OrderedMenuItems",
                column: "OrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderedMenuItems",
                table: "OrderedMenuItems");

            migrationBuilder.DropIndex(
                name: "IX_OrderedMenuItems_OrderId",
                table: "OrderedMenuItems");

            migrationBuilder.DropColumn(
                name: "OrderedMenuItemId",
                table: "OrderedMenuItems");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderedMenuItems",
                table: "OrderedMenuItems",
                columns: new[] { "OrderId", "MenuId", "MenuItemType" });
        }
    }
}
