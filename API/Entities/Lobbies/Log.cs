namespace API.Entities.Lobbies
{
    public class Log
    {
        public int Id { get; set; }
        public ICollection<Message> Messages { get; set; }
    }
}