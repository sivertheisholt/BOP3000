using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    public partial class MovedFinishedLobbies : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FinishedLobbies",
                table: "AppUserProfile");

            migrationBuilder.AddColumn<string>(
                name: "FinishedLobbies",
                table: "AppUserData",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FinishedLobbies",
                table: "AppUserData");

            migrationBuilder.AddColumn<string>(
                name: "FinishedLobbies",
                table: "AppUserProfile",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
