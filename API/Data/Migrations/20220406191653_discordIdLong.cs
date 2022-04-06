using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    public partial class discordIdLong : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "DiscordId",
                table: "DiscordProfile",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(20,0)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "DiscordId",
                table: "DiscordProfile",
                type: "decimal(20,0)",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");
        }
    }
}
