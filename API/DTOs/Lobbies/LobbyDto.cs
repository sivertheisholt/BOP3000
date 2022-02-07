namespace API.DTOs.Lobbies
{
    public class LobbyDto
    {
        public int Id { get; set; }
        public int MaxUsers { get; set; }
        public string Title { get; set; }
        public string SteamId { get; set; }

        public ICollection<int> Users { get; set; }
        public RequirementDto LobbyRequirement { get; set; }
    }
}