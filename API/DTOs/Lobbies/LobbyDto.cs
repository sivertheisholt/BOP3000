namespace API.DTOs.Lobbies
{
    public class LobbyDto
    {
        public int Id { get; set; }
        public int MaxUsers { get; set; }
        public string Title { get; set; }
        public int GameId { get; set; }
        public string GameName { get; set; }
        public string GameType { get; set; }
        public string lobbyDescription { get; set; }
        public int AdminUid { get; set; }
        public string AdminUsername { get; set; }
        public string AdminProfilePic { get; set; }
        public ICollection<int> Users { get; set; }
        public LogDto Log { get; set; }
        public RequirementDto LobbyRequirement { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishedDate { get; set; }
        public bool Finished { get; set; }
    }
}