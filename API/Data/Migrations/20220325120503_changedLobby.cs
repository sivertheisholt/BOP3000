using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    public partial class changedLobby : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Lobby",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "FinishedDate",
                table: "Lobby",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "logId",
                table: "Lobby",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Log",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Log", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Lobby_logId",
                table: "Lobby",
                column: "logId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lobby_Log_logId",
                table: "Lobby",
                column: "logId",
                principalTable: "Log",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lobby_Log_logId",
                table: "Lobby");

            migrationBuilder.DropTable(
                name: "Log");

            migrationBuilder.DropIndex(
                name: "IX_Lobby_logId",
                table: "Lobby");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Lobby");

            migrationBuilder.DropColumn(
                name: "FinishedDate",
                table: "Lobby");

            migrationBuilder.DropColumn(
                name: "logId",
                table: "Lobby");
        }
    }
}
