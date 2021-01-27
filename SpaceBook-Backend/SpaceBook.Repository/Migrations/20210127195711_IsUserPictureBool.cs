using Microsoft.EntityFrameworkCore.Migrations;

namespace SpaceBook.Repository.Migrations
{
    public partial class IsUserPictureBool : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isUserPicture",
                table: "Pictures",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isUserPicture",
                table: "Pictures");
        }
    }
}
