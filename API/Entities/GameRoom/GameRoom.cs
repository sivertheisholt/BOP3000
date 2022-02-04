using System.ComponentModel.DataAnnotations;
using API.Entities.Users;

namespace API.Entities.GameRoom
{
    public class GameRoom
    {
        public int Id { get; set; }
        public int MaxUsers { get; set; }
        public int AdminUid { get; set; }
        public string Title { get; set; }
        public string LobbyDescription { get; set; }
        public int GameId { get; set; }
        public string GameType { get; set; }
        public ICollection<AppUser> Users { get; set; }
        public Requirement RoomRequirement { get; set; }
    }
}