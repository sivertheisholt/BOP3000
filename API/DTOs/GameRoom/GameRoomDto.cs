using API.Entities.GameRoom;

namespace API.DTOs.GameRoom
{
    public class GameRoomDto
    {
        public int Id { get; set; }
        public int MaxUsers { get; set; }
        public int AdminUid { get; set; }
        public string Title { get; set; }
        public string SteamId { get; set; }

        public ICollection<int> Users { get; set; }
        public RequirementDto RoomRequirement { get; set; }
    }
}