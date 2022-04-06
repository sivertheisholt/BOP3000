using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    public partial class discordProfile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Distriminator",
                table: "DiscordProfile",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Hidden",
                table: "DiscordProfile",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "DiscordProfile",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Distriminator",
                table: "DiscordProfile");

            migrationBuilder.DropColumn(
                name: "Hidden",
                table: "DiscordProfile");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "DiscordProfile");
        }
    }
}
