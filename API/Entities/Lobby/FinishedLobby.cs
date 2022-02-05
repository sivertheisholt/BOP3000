namespace API.Entities.Lobby
{
    public class FinishedLobby : Lobby
    {
        public Log log { get; set; }
        public DateTime FinishedDate { get; set; }
    }
}