using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace glissvinyls_plus.Migrations
{
    /// <inheritdoc />
    public partial class ChangeExitDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ExitDetails_ProductId",
                table: "ExitDetails",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExitDetails_Products_ProductId",
                table: "ExitDetails",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExitDetails_Products_ProductId",
                table: "ExitDetails");

            migrationBuilder.DropIndex(
                name: "IX_ExitDetails_ProductId",
                table: "ExitDetails");
        }
    }
}
