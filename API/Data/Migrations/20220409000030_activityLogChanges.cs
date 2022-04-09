using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    public partial class activityLogChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LobbyId",
                table: "ActivityLog",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MemberFollowedId",
                table: "ActivityLog",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LobbyId",
                table: "ActivityLog");

            migrationBuilder.DropColumn(
                name: "MemberFollowedId",
                table: "ActivityLog");
        }
    }
}
