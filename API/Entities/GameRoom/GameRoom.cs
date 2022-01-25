namespace API.Entities.GameRoom
{
    public class GameRoom
    {
        public int Rid { get; set; }
        public Users Users { get; set; }
        public int MaxUsers { get; set; }
        public int AdminUid { get; set; }
        public string Title { get; set; }

        public Requirement RoomRequirement { get; set; }
    }
}