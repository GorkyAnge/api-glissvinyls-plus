using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace glissvinyls_plus.Migrations
{
    /// <inheritdoc />
    public partial class AddWareHouseIdToInventoryExit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WarehouseId",
                table: "InventoryExits",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_InventoryExits_WarehouseId",
                table: "InventoryExits",
                column: "WarehouseId");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryExits_Warehouses_WarehouseId",
                table: "InventoryExits",
                column: "WarehouseId",
                principalTable: "Warehouses",
                principalColumn: "WarehouseId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryExits_Warehouses_WarehouseId",
                table: "InventoryExits");

            migrationBuilder.DropIndex(
                name: "IX_InventoryExits_WarehouseId",
                table: "InventoryExits");

            migrationBuilder.DropColumn(
                name: "WarehouseId",
                table: "InventoryExits");
        }
    }
}
