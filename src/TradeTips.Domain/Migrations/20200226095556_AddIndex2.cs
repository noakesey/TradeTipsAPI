using Microsoft.EntityFrameworkCore.Migrations;

namespace TradeTips.Domain.Migrations
{
    public partial class AddIndex2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_IntraDayPrices_TransDate",
                table: "IntraDayPrices",
                column: "TransDate");

            migrationBuilder.CreateIndex(
                name: "IX_DailyPrices_TransDate",
                table: "DailyPrices",
                column: "TransDate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_IntraDayPrices_TransDate",
                table: "IntraDayPrices");

            migrationBuilder.DropIndex(
                name: "IX_DailyPrices_TransDate",
                table: "DailyPrices");
        }
    }
}
