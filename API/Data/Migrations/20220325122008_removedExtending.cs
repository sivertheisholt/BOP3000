using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    public partial class removedExtending : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lobby_Log_logId",
                table: "Lobby");

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
                name: "Users",
                table: "Lobby");

            migrationBuilder.DropColumn(
                name: "logId",
                table: "Lobby");

            migrationBuilder.CreateTable(
                name: "FinishedLobby",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaxUsers = table.Column<int>(type: "int", nullable: false),
                    AdminUid = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LobbyDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GameId = table.Column<int>(type: "int", nullable: false),
                    GameType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LobbyRequirementId = table.Column<int>(type: "int", nullable: true),
                    logId = table.Column<int>(type: "int", nullable: true),
                    FinishedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Users = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinishedLobby", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FinishedLobby_Log_logId",
                        column: x => x.logId,
                        principalTable: "Log",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FinishedLobby_Requirement_LobbyRequirementId",
                        column: x => x.LobbyRequirementId,
                        principalTable: "Requirement",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FinishedLobby_LobbyRequirementId",
                table: "FinishedLobby",
                column: "LobbyRequirementId");

            migrationBuilder.CreateIndex(
                name: "IX_FinishedLobby_logId",
                table: "FinishedLobby",
                column: "logId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FinishedLobby");

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

            migrationBuilder.AddColumn<string>(
                name: "Users",
                table: "Lobby",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "logId",
                table: "Lobby",
                type: "int",
                nullable: true);

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
    }
}
