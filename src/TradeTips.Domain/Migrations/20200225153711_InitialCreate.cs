using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TradeTips.Domain.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    PersonId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Bio = table.Column<string>(nullable: true),
                    Image = table.Column<string>(nullable: true),
                    Hash = table.Column<byte[]>(nullable: true),
                    Salt = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.PersonId);
                });

            migrationBuilder.CreateTable(
                name: "Stocks",
                columns: table => new
                {
                    StockId = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Obsolete = table.Column<bool>(nullable: false),
                    BargainShare = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stocks", x => x.StockId);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    TagId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.TagId);
                });

            migrationBuilder.CreateTable(
                name: "FollowedPeople",
                columns: table => new
                {
                    ObserverId = table.Column<int>(nullable: false),
                    TargetId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FollowedPeople", x => new { x.ObserverId, x.TargetId });
                    table.ForeignKey(
                        name: "FK_FollowedPeople_Persons_ObserverId",
                        column: x => x.ObserverId,
                        principalTable: "Persons",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FollowedPeople_Persons_TargetId",
                        column: x => x.TargetId,
                        principalTable: "Persons",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Articles",
                columns: table => new
                {
                    ArticleId = table.Column<int>(nullable: false),
                    Slug = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Teaser = table.Column<string>(nullable: true),
                    Summary = table.Column<string>(nullable: true),
                    PublicationDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    Link = table.Column<string>(nullable: true),
                    StockLink = table.Column<string>(nullable: true),
                    StockId = table.Column<string>(nullable: true),
                    IsAlpha = table.Column<bool>(nullable: false),
                    Price = table.Column<decimal>(type: "decimal(12, 5)", nullable: false),
                    PriceTMinus7 = table.Column<decimal>(type: "decimal(12, 5)", nullable: true),
                    PriceTMinus1 = table.Column<decimal>(type: "decimal(12, 5)", nullable: true),
                    PriceOpen = table.Column<decimal>(type: "decimal(12, 5)", nullable: true),
                    PriceClose = table.Column<decimal>(type: "decimal(12, 5)", nullable: true),
                    PriceTPlus1 = table.Column<decimal>(type: "decimal(12, 5)", nullable: true),
                    PriceTPlus2 = table.Column<decimal>(type: "decimal(12, 5)", nullable: true),
                    PriceTPlus3 = table.Column<decimal>(type: "decimal(12, 5)", nullable: true),
                    PriceTPlus5 = table.Column<decimal>(type: "decimal(12, 5)", nullable: true),
                    PriceTPlus7 = table.Column<decimal>(type: "decimal(12, 5)", nullable: true),
                    AuthorPersonId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articles", x => x.ArticleId);
                    table.ForeignKey(
                        name: "FK_Articles_Persons_AuthorPersonId",
                        column: x => x.AuthorPersonId,
                        principalTable: "Persons",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Articles_Stocks_StockId",
                        column: x => x.StockId,
                        principalTable: "Stocks",
                        principalColumn: "StockId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DailyPrices",
                columns: table => new
                {
                    DailyPriceId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StockId = table.Column<string>(nullable: true),
                    TransDate = table.Column<DateTime>(nullable: false),
                    Open = table.Column<decimal>(type: "decimal(12, 5)", nullable: false),
                    High = table.Column<decimal>(type: "decimal(12, 5)", nullable: false),
                    Low = table.Column<decimal>(type: "decimal(12, 5)", nullable: false),
                    Close = table.Column<decimal>(type: "decimal(12, 5)", nullable: false),
                    Volume = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyPrices", x => x.DailyPriceId);
                    table.ForeignKey(
                        name: "FK_DailyPrices_Stocks_StockId",
                        column: x => x.StockId,
                        principalTable: "Stocks",
                        principalColumn: "StockId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IntraDayPrices",
                columns: table => new
                {
                    IntraDayPriceId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StockId = table.Column<string>(nullable: true),
                    TransDate = table.Column<DateTime>(nullable: false),
                    High = table.Column<decimal>(type: "decimal(12, 5)", nullable: false),
                    Low = table.Column<decimal>(type: "decimal(12, 5)", nullable: false),
                    Volume = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntraDayPrices", x => x.IntraDayPriceId);
                    table.ForeignKey(
                        name: "FK_IntraDayPrices_Stocks_StockId",
                        column: x => x.StockId,
                        principalTable: "Stocks",
                        principalColumn: "StockId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ArticleFavorites",
                columns: table => new
                {
                    ArticleId = table.Column<int>(nullable: false),
                    PersonId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleFavorites", x => new { x.ArticleId, x.PersonId });
                    table.ForeignKey(
                        name: "FK_ArticleFavorites_Articles_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Articles",
                        principalColumn: "ArticleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArticleFavorites_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ArticleTags",
                columns: table => new
                {
                    ArticleId = table.Column<int>(nullable: false),
                    TagId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleTags", x => new { x.ArticleId, x.TagId });
                    table.ForeignKey(
                        name: "FK_ArticleTags_Articles_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Articles",
                        principalColumn: "ArticleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArticleTags_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "TagId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    CommentId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Body = table.Column<string>(nullable: true),
                    AuthorId = table.Column<int>(nullable: false),
                    ArticleId = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.CommentId);
                    table.ForeignKey(
                        name: "FK_Comments_Articles_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Articles",
                        principalColumn: "ArticleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comments_Persons_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Persons",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArticleFavorites_PersonId",
                table: "ArticleFavorites",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_AuthorPersonId",
                table: "Articles",
                column: "AuthorPersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_StockId",
                table: "Articles",
                column: "StockId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleTags_TagId",
                table: "ArticleTags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ArticleId",
                table: "Comments",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_AuthorId",
                table: "Comments",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyPrices_StockId",
                table: "DailyPrices",
                column: "StockId");

            migrationBuilder.CreateIndex(
                name: "IX_FollowedPeople_TargetId",
                table: "FollowedPeople",
                column: "TargetId");

            migrationBuilder.CreateIndex(
                name: "IX_IntraDayPrices_StockId",
                table: "IntraDayPrices",
                column: "StockId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArticleFavorites");

            migrationBuilder.DropTable(
                name: "ArticleTags");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "DailyPrices");

            migrationBuilder.DropTable(
                name: "FollowedPeople");

            migrationBuilder.DropTable(
                name: "IntraDayPrices");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Articles");

            migrationBuilder.DropTable(
                name: "Persons");

            migrationBuilder.DropTable(
                name: "Stocks");
        }
    }
}
