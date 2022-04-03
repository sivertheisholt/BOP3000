using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    public partial class photoToProfile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppUserPhoto_AspNetUsers_AppUserId",
                table: "AppUserPhoto");

            migrationBuilder.DropForeignKey(
                name: "FK_AppUserProfile_AppUserPhoto_AppUserPhotoId",
                table: "AppUserProfile");

            migrationBuilder.DropIndex(
                name: "IX_AppUserProfile_AppUserPhotoId",
                table: "AppUserProfile");

            migrationBuilder.DropIndex(
                name: "IX_AppUserPhoto_AppUserId",
                table: "AppUserPhoto");

            migrationBuilder.DropColumn(
                name: "AppUserPhotoId",
                table: "AppUserProfile");

            migrationBuilder.RenameColumn(
                name: "AppUserId",
                table: "AppUserPhoto",
                newName: "AppUserProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_AppUserPhoto_AppUserProfileId",
                table: "AppUserPhoto",
                column: "AppUserProfileId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserPhoto_AppUserProfile_AppUserProfileId",
                table: "AppUserPhoto",
                column: "AppUserProfileId",
                principalTable: "AppUserProfile",
                principalColumn: "AppUserId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppUserPhoto_AppUserProfile_AppUserProfileId",
                table: "AppUserPhoto");

            migrationBuilder.DropIndex(
                name: "IX_AppUserPhoto_AppUserProfileId",
                table: "AppUserPhoto");

            migrationBuilder.RenameColumn(
                name: "AppUserProfileId",
                table: "AppUserPhoto",
                newName: "AppUserId");

            migrationBuilder.AddColumn<int>(
                name: "AppUserPhotoId",
                table: "AppUserProfile",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppUserProfile_AppUserPhotoId",
                table: "AppUserProfile",
                column: "AppUserPhotoId");

            migrationBuilder.CreateIndex(
                name: "IX_AppUserPhoto_AppUserId",
                table: "AppUserPhoto",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserPhoto_AspNetUsers_AppUserId",
                table: "AppUserPhoto",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserProfile_AppUserPhoto_AppUserPhotoId",
                table: "AppUserProfile",
                column: "AppUserPhotoId",
                principalTable: "AppUserPhoto",
                principalColumn: "Id");
        }
    }
}
