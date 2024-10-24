using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sotex.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddedMenuItemTypeAsForeignKeyToOrderedMenuItemTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderedMenuItems",
                table: "OrderedMenuItems");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalPrice",
                table: "Orders",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderedMenuItems",
                table: "OrderedMenuItems",
                columns: new[] { "OrderId", "MenuId", "MenuItemType" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderedMenuItems",
                table: "OrderedMenuItems");

            migrationBuilder.AlterColumn<int>(
                name: "TotalPrice",
                table: "Orders",
                type: "integer",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderedMenuItems",
                table: "OrderedMenuItems",
                columns: new[] { "OrderId", "MenuId" });
        }
    }
}
