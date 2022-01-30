using System.ComponentModel.DataAnnotations;

namespace API.Entities.SteamApp
{
    public class GameInfo
    {
        [Key]
        public int id { get; set; }
        public bool success { get; set; }
        public Information.Data data { get; set; }
    }
}