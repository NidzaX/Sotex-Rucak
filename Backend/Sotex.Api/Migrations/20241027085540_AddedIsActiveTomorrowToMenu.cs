using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sotex.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddedIsActiveTomorrowToMenu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActiveTomorrow",
                table: "Menus",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActiveTomorrow",
                table: "Menus");
        }
    }
}
