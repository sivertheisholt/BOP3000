using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    public partial class AddedMoreLobbyInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FinishedLobby_Log_logId",
                table: "FinishedLobby");

            migrationBuilder.RenameColumn(
                name: "logId",
                table: "FinishedLobby",
                newName: "LogId");

            migrationBuilder.RenameIndex(
                name: "IX_FinishedLobby_logId",
                table: "FinishedLobby",
                newName: "IX_FinishedLobby_LogId");

            migrationBuilder.AddColumn<string>(
                name: "GameName",
                table: "Lobby",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "Lobby",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_FinishedLobby_Log_LogId",
                table: "FinishedLobby",
                column: "LogId",
                principalTable: "Log",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FinishedLobby_Log_LogId",
                table: "FinishedLobby");

            migrationBuilder.DropColumn(
                name: "GameName",
                table: "Lobby");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Lobby");

            migrationBuilder.RenameColumn(
                name: "LogId",
                table: "FinishedLobby",
                newName: "logId");

            migrationBuilder.RenameIndex(
                name: "IX_FinishedLobby_LogId",
                table: "FinishedLobby",
                newName: "IX_FinishedLobby_logId");

            migrationBuilder.AddForeignKey(
                name: "FK_FinishedLobby_Log_logId",
                table: "FinishedLobby",
                column: "logId",
                principalTable: "Log",
                principalColumn: "Id");
        }
    }
}
