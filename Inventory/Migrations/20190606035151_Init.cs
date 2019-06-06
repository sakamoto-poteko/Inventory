using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Inventory.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Footprints",
                columns: table => new
                {
                    FootprintId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FootprintName = table.Column<string>(nullable: false),
                    Comments = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Footprints", x => x.FootprintId);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    LocationId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    LocationName = table.Column<string>(nullable: false),
                    LocationUnit = table.Column<string>(nullable: true),
                    Comments = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.LocationId);
                });

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    Comments = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    ProductId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProductName = table.Column<string>(nullable: false),
                    FootprintId = table.Column<int>(nullable: true),
                    Manufacturer = table.Column<string>(nullable: true),
                    Comments = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.ProductId);
                    table.ForeignKey(
                        name: "FK_Product_Footprints_FootprintId",
                        column: x => x.FootprintId,
                        principalTable: "Footprints",
                        principalColumn: "FootprintId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Inventories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProductId = table.Column<int>(nullable: false),
                    LocationId = table.Column<int>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    Comments = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Inventories_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "LocationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Inventories_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    InventoryId = table.Column<int>(nullable: false),
                    Direction = table.Column<int>(nullable: false),
                    SupplierId = table.Column<int>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    Price = table.Column<int>(nullable: false),
                    Comments = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_Inventories_InventoryId",
                        column: x => x.InventoryId,
                        principalTable: "Inventories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transactions_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Footprints_FootprintName",
                table: "Footprints",
                column: "FootprintName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Inventories_LocationId",
                table: "Inventories",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Inventories_ProductId_LocationId",
                table: "Inventories",
                columns: new[] { "ProductId", "LocationId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Locations_LocationName",
                table: "Locations",
                column: "LocationName");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_LocationName_LocationUnit",
                table: "Locations",
                columns: new[] { "LocationName", "LocationUnit" },
                unique: true,
                filter: "[LocationUnit] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Product_FootprintId",
                table: "Product",
                column: "FootprintId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_Manufacturer",
                table: "Product",
                column: "Manufacturer");

            migrationBuilder.CreateIndex(
                name: "IX_Product_ProductName_FootprintId",
                table: "Product",
                columns: new[] { "ProductName", "FootprintId" },
                unique: true,
                filter: "[FootprintId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_Name",
                table: "Suppliers",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_InventoryId",
                table: "Transactions",
                column: "InventoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_SupplierId",
                table: "Transactions",
                column: "SupplierId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "Inventories");

            migrationBuilder.DropTable(
                name: "Suppliers");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "Footprints");
        }
    }
}
