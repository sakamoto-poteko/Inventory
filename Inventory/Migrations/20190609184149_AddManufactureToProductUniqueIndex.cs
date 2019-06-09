using Microsoft.EntityFrameworkCore.Migrations;

namespace Inventory.Migrations
{
    public partial class AddManufactureToProductUniqueIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Products_ProductName_FootprintId",
                table: "Products");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductName_FootprintId_Manufacturer",
                table: "Products",
                columns: new[] { "ProductName", "FootprintId", "Manufacturer" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Products_ProductName_FootprintId_Manufacturer",
                table: "Products");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductName_FootprintId",
                table: "Products",
                columns: new[] { "ProductName", "FootprintId" },
                unique: true);
        }
    }
}
