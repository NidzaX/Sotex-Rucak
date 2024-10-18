using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sotex.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddedOrderPhoneAndOrderNoteColumnsToMenu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ContactInfo",
                table: "Menus",
                newName: "OrderPhone");

            migrationBuilder.RenameColumn(
                name: "AdditionalInfo",
                table: "Menus",
                newName: "OrderNote");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OrderPhone",
                table: "Menus",
                newName: "ContactInfo");

            migrationBuilder.RenameColumn(
                name: "OrderNote",
                table: "Menus",
                newName: "AdditionalInfo");
        }
    }
}
