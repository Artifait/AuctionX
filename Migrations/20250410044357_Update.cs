using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AucX.Migrations
{
    /// <inheritdoc />
    public partial class Update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CanvasItems_AspNetUsers_UserId",
                table: "CanvasItems");

            migrationBuilder.RenameColumn(
                name: "ColorCodes",
                table: "CanvasItems",
                newName: "PixelData");

            migrationBuilder.RenameIndex(
                name: "IX_CanvasItems_UserId_Width_Height_ColorCodes",
                table: "CanvasItems",
                newName: "IX_CanvasItems_UserId_Width_Height_PixelData");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PixelData",
                table: "CanvasItems",
                newName: "ColorCodes");

            migrationBuilder.RenameIndex(
                name: "IX_CanvasItems_UserId_Width_Height_PixelData",
                table: "CanvasItems",
                newName: "IX_CanvasItems_UserId_Width_Height_ColorCodes");

            migrationBuilder.AddForeignKey(
                name: "FK_CanvasItems_AspNetUsers_UserId",
                table: "CanvasItems",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
