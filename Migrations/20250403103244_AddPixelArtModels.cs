using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AucX.Migrations
{
    /// <inheritdoc />
    public partial class AddPixelArtModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuctionLots");

            migrationBuilder.DropTable(
                name: "GameItems");

            migrationBuilder.CreateTable(
                name: "ColorShops",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Price = table.Column<decimal>(type: "TEXT", nullable: false),
                    ColorCode = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ColorShops", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PixelArts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Width = table.Column<int>(type: "INTEGER", nullable: false),
                    Height = table.Column<int>(type: "INTEGER", nullable: false),
                    OwnerId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PixelArts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PixelArts_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserCanvasUpgrades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    MaxWidth = table.Column<int>(type: "INTEGER", nullable: false),
                    MaxHeight = table.Column<int>(type: "INTEGER", nullable: false),
                    Price = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCanvasUpgrades", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserCanvasUpgrades_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserColorPurchases",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    ColorShopId = table.Column<int>(type: "INTEGER", nullable: false),
                    DatePurchased = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserColorPurchases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserColorPurchases_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserColorPurchases_ColorShops_ColorShopId",
                        column: x => x.ColorShopId,
                        principalTable: "ColorShops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PixelColors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ColorCode = table.Column<string>(type: "TEXT", nullable: false),
                    PixelArtId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PixelColors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PixelColors_PixelArts_PixelArtId",
                        column: x => x.PixelArtId,
                        principalTable: "PixelArts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PixelArts_OwnerId",
                table: "PixelArts",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_PixelColors_PixelArtId",
                table: "PixelColors",
                column: "PixelArtId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCanvasUpgrades_UserId",
                table: "UserCanvasUpgrades",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserColorPurchases_ColorShopId",
                table: "UserColorPurchases",
                column: "ColorShopId");

            migrationBuilder.CreateIndex(
                name: "IX_UserColorPurchases_UserId",
                table: "UserColorPurchases",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PixelColors");

            migrationBuilder.DropTable(
                name: "UserCanvasUpgrades");

            migrationBuilder.DropTable(
                name: "UserColorPurchases");

            migrationBuilder.DropTable(
                name: "PixelArts");

            migrationBuilder.DropTable(
                name: "ColorShops");

            migrationBuilder.CreateTable(
                name: "GameItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    ImageUrl = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuctionLots",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GameItemId = table.Column<int>(type: "INTEGER", nullable: false),
                    BuyOutPrice = table.Column<decimal>(type: "TEXT", nullable: false),
                    CurrentBid = table.Column<decimal>(type: "TEXT", nullable: false),
                    EndTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    MinBidIncrement = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuctionLots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuctionLots_GameItems_GameItemId",
                        column: x => x.GameItemId,
                        principalTable: "GameItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuctionLots_GameItemId",
                table: "AuctionLots",
                column: "GameItemId");
        }
    }
}
