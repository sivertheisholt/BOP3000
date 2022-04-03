using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    public partial class profilePhoto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AppUserPhotoId",
                table: "AppUserProfile",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AppUserPhoto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PublicId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AppUserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserPhoto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppUserPhoto_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppUserProfile_AppUserPhotoId",
                table: "AppUserProfile",
                column: "AppUserPhotoId");

            migrationBuilder.CreateIndex(
                name: "IX_AppUserPhoto_AppUserId",
                table: "AppUserPhoto",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserProfile_AppUserPhoto_AppUserPhotoId",
                table: "AppUserProfile",
                column: "AppUserPhotoId",
                principalTable: "AppUserPhoto",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppUserProfile_AppUserPhoto_AppUserPhotoId",
                table: "AppUserProfile");

            migrationBuilder.DropTable(
                name: "AppUserPhoto");

            migrationBuilder.DropIndex(
                name: "IX_AppUserProfile_AppUserPhotoId",
                table: "AppUserProfile");

            migrationBuilder.DropColumn(
                name: "AppUserPhotoId",
                table: "AppUserProfile");
        }
    }
}
