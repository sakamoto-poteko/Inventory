using Microsoft.EntityFrameworkCore.Migrations;

namespace Inventory.Migrations
{
    public partial class RemoveNullableUniqueIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Product_ProductName_FootprintId",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_Locations_LocationName_LocationUnit",
                table: "Locations");

            migrationBuilder.CreateIndex(
                name: "IX_Product_ProductName_FootprintId",
                table: "Product",
                columns: new[] { "ProductName", "FootprintId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Locations_LocationName_LocationUnit",
                table: "Locations",
                columns: new[] { "LocationName", "LocationUnit" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Product_ProductName_FootprintId",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_Locations_LocationName_LocationUnit",
                table: "Locations");

            migrationBuilder.CreateIndex(
                name: "IX_Product_ProductName_FootprintId",
                table: "Product",
                columns: new[] { "ProductName", "FootprintId" },
                unique: true,
                filter: "[FootprintId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_LocationName_LocationUnit",
                table: "Locations",
                columns: new[] { "LocationName", "LocationUnit" },
                unique: true,
                filter: "[LocationUnit] IS NOT NULL");
        }
    }
}
