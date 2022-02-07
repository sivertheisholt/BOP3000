using API.Entities.Lobbies;

namespace API.Entities.Users
{
    public class AppUserProfile
    {
        public int Id { get; set; }
        public AppUserConnections UserConnections { get; set; }
        public string Nickname { get; set; }
        public ICollection<FinishedLobby> FinishedLobbies { get; set; }
    }
}