using API.Entities.Users;

namespace API.Entities.Lobbies
{
    public class Lobby
    {
        public int Id { get; set; }
        public int MaxUsers { get; set; }
        public int AdminUid { get; set; }
        public string Title { get; set; }
        public string LobbyDescription { get; set; }
        public int GameId { get; set; }
        public string GameName { get; set; }
        public string GameType { get; set; }
        public DateTime FinishedDate { get; set; }
        public Log Log { get; set; }
        public Requirement LobbyRequirement { get; set; }
        public ICollection<int> Users { get; set; }
        public DateTime StartDate { get; set; }
        public bool Finished { get; set; }
    }
}