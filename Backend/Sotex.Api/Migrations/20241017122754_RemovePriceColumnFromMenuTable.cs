using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sotex.Api.Migrations
{
    /// <inheritdoc />
    public partial class RemovePriceColumnFromMenuTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Menus");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Menus",
                type: "numeric",
                nullable: true);
        }
    }
}
