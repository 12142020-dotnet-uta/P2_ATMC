using Microsoft.EntityFrameworkCore.Migrations;

namespace SpaceBook.Repository.Migrations
{
    public partial class PictureRegularPhotoAndHD : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageHDURL",
                table: "Pictures",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageHDURL",
                table: "Pictures");
        }
    }
}
