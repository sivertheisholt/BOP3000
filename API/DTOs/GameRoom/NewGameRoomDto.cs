
using API.DTOs.GameRoom;

namespace API.DTOs.GameRoom
{
    public class NewGameRoomDto
    {
        public int MaxUsers { get; set; }
        public int AdminUid { get; set; }
        public string Title { get; set; }
        public RequirementDto RoomRequirement { get; set; }
    }
}