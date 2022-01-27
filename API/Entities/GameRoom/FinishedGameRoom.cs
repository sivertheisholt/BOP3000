namespace API.Entities.GameRoom
{
    public class FinishedGameRoom : GameRoom
    {
        public Log log { get; set; }
        public DateTime FinishedDate { get; set; }
    }
}