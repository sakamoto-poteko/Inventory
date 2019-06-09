using Microsoft.EntityFrameworkCore.Migrations;

namespace Inventory.Migrations
{
    public partial class AddProductsToDbSet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inventories_Product_ProductId",
                table: "Inventories");

            migrationBuilder.DropForeignKey(
                name: "FK_Product_Footprints_FootprintId",
                table: "Product");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Product",
                table: "Product");

            migrationBuilder.RenameTable(
                name: "Product",
                newName: "Products");

            migrationBuilder.RenameIndex(
                name: "IX_Product_ProductName_FootprintId",
                table: "Products",
                newName: "IX_Products_ProductName_FootprintId");

            migrationBuilder.RenameIndex(
                name: "IX_Product_Manufacturer",
                table: "Products",
                newName: "IX_Products_Manufacturer");

            migrationBuilder.RenameIndex(
                name: "IX_Product_FootprintId",
                table: "Products",
                newName: "IX_Products_FootprintId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Products",
                table: "Products",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Inventories_Products_ProductId",
                table: "Inventories",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Footprints_FootprintId",
                table: "Products",
                column: "FootprintId",
                principalTable: "Footprints",
                principalColumn: "FootprintId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inventories_Products_ProductId",
                table: "Inventories");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Footprints_FootprintId",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Products",
                table: "Products");

            migrationBuilder.RenameTable(
                name: "Products",
                newName: "Product");

            migrationBuilder.RenameIndex(
                name: "IX_Products_ProductName_FootprintId",
                table: "Product",
                newName: "IX_Product_ProductName_FootprintId");

            migrationBuilder.RenameIndex(
                name: "IX_Products_Manufacturer",
                table: "Product",
                newName: "IX_Product_Manufacturer");

            migrationBuilder.RenameIndex(
                name: "IX_Products_FootprintId",
                table: "Product",
                newName: "IX_Product_FootprintId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Product",
                table: "Product",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Inventories_Product_ProductId",
                table: "Inventories",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Footprints_FootprintId",
                table: "Product",
                column: "FootprintId",
                principalTable: "Footprints",
                principalColumn: "FootprintId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
