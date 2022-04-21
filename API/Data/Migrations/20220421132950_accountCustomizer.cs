using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    public partial class accountCustomizer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppUserCustomization",
                columns: table => new
                {
                    AppUserProfileId = table.Column<int>(type: "int", nullable: false),
                    BackgroundUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserCustomization", x => x.AppUserProfileId);
                    table.ForeignKey(
                        name: "FK_AppUserCustomization_AppUserProfile_AppUserProfileId",
                        column: x => x.AppUserProfileId,
                        principalTable: "AppUserProfile",
                        principalColumn: "AppUserId",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppUserCustomization");
        }
    }
}
