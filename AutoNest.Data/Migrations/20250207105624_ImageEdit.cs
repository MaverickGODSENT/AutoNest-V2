using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoNest.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class ImageEdit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageId",
                table: "Parts",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Parts_ImageId",
                table: "Parts",
                column: "ImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Parts_Images_ImageId",
                table: "Parts",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Parts_Images_ImageId",
                table: "Parts");

            migrationBuilder.DropIndex(
                name: "IX_Parts_ImageId",
                table: "Parts");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "Parts");
        }
    }
}
