using Microsoft.EntityFrameworkCore.Migrations;

namespace SpaceBook.Repository.Migrations
{
    public partial class m3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_AspNetUsers_UserCommentedId1",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Favorites_AspNetUsers_UserId1",
                table: "Favorites");

            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_AspNetUsers_UserRatingId1",
                table: "Ratings");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPictures_AspNetUsers_UploadedById1",
                table: "UserPictures");

            migrationBuilder.DropIndex(
                name: "IX_UserPictures_UploadedById1",
                table: "UserPictures");

            migrationBuilder.DropIndex(
                name: "IX_Ratings_UserRatingId1",
                table: "Ratings");

            migrationBuilder.DropIndex(
                name: "IX_Favorites_UserId1",
                table: "Favorites");

            migrationBuilder.DropIndex(
                name: "IX_Comments_UserCommentedId1",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "UploadedById1",
                table: "UserPictures");

            migrationBuilder.DropColumn(
                name: "UserRatingId1",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Favorites");

            migrationBuilder.DropColumn(
                name: "UserCommentedId1",
                table: "Comments");

            migrationBuilder.AlterColumn<string>(
                name: "UploadedById",
                table: "UserPictures",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "UserRatingId",
                table: "Ratings",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Favorites",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "UserCommentedId",
                table: "Comments",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_UserPictures_UploadedById",
                table: "UserPictures",
                column: "UploadedById");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_UserRatingId",
                table: "Ratings",
                column: "UserRatingId");

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_UserId",
                table: "Favorites",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserCommentedId",
                table: "Comments",
                column: "UserCommentedId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_AspNetUsers_UserCommentedId",
                table: "Comments",
                column: "UserCommentedId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Favorites_AspNetUsers_UserId",
                table: "Favorites",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_AspNetUsers_UserRatingId",
                table: "Ratings",
                column: "UserRatingId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserPictures_AspNetUsers_UploadedById",
                table: "UserPictures",
                column: "UploadedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_AspNetUsers_UserCommentedId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Favorites_AspNetUsers_UserId",
                table: "Favorites");

            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_AspNetUsers_UserRatingId",
                table: "Ratings");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPictures_AspNetUsers_UploadedById",
                table: "UserPictures");

            migrationBuilder.DropIndex(
                name: "IX_UserPictures_UploadedById",
                table: "UserPictures");

            migrationBuilder.DropIndex(
                name: "IX_Ratings_UserRatingId",
                table: "Ratings");

            migrationBuilder.DropIndex(
                name: "IX_Favorites_UserId",
                table: "Favorites");

            migrationBuilder.DropIndex(
                name: "IX_Comments_UserCommentedId",
                table: "Comments");

            migrationBuilder.AlterColumn<int>(
                name: "UploadedById",
                table: "UserPictures",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UploadedById1",
                table: "UserPictures",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UserRatingId",
                table: "Ratings",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserRatingId1",
                table: "Ratings",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Favorites",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "Favorites",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UserCommentedId",
                table: "Comments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserCommentedId1",
                table: "Comments",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserPictures_UploadedById1",
                table: "UserPictures",
                column: "UploadedById1");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_UserRatingId1",
                table: "Ratings",
                column: "UserRatingId1");

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_UserId1",
                table: "Favorites",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserCommentedId1",
                table: "Comments",
                column: "UserCommentedId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_AspNetUsers_UserCommentedId1",
                table: "Comments",
                column: "UserCommentedId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Favorites_AspNetUsers_UserId1",
                table: "Favorites",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_AspNetUsers_UserRatingId1",
                table: "Ratings",
                column: "UserRatingId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserPictures_AspNetUsers_UploadedById1",
                table: "UserPictures",
                column: "UploadedById1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
