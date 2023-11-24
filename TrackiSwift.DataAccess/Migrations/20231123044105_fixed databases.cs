using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrackiSwift.Migrations
{
    /// <inheritdoc />
    public partial class fixeddatabases : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_OrderBackups_OrderId",
                table: "OrderBackups",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderBackups_Orders_OrderId",
                table: "OrderBackups",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "OrderId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderBackups_Orders_OrderId",
                table: "OrderBackups");

            migrationBuilder.DropIndex(
                name: "IX_OrderBackups_OrderId",
                table: "OrderBackups");
        }
    }
}
