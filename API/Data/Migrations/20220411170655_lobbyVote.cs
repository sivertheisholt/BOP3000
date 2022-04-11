using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    public partial class lobbyVote : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LobbyVote",
                columns: table => new
                {
                    LobbyVoteId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VoterUid = table.Column<int>(type: "int", nullable: false),
                    VotedUid = table.Column<int>(type: "int", nullable: false),
                    Upvote = table.Column<bool>(type: "bit", nullable: false),
                    LobbyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LobbyVote", x => x.LobbyVoteId);
                    table.ForeignKey(
                        name: "FK_LobbyVote_Lobby_LobbyId",
                        column: x => x.LobbyId,
                        principalTable: "Lobby",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LobbyVote_LobbyId",
                table: "LobbyVote",
                column: "LobbyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LobbyVote");
        }
    }
}
