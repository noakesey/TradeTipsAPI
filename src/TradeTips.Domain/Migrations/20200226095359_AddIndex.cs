using Microsoft.EntityFrameworkCore.Migrations;

namespace TradeTips.Domain.Migrations
{
    public partial class AddIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Articles_PublicationDate",
                table: "Articles",
                column: "PublicationDate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Articles_PublicationDate",
                table: "Articles");
        }
    }
}
