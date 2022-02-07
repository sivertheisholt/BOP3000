
namespace API.DTOs.Lobbies
{
    public class NewLobbyDto
    {
        public int MaxUsers { get; set; }
        public string Title { get; set; }
        public string LobbyDescription { get; set; }
        public int GameId { get; set; }
        public string GameType { get; set; }
        public RequirementDto LobbyRequirement { get; set; }
    }
}