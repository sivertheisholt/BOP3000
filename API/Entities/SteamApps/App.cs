using System.ComponentModel.DataAnnotations;

namespace API.Entities.SteamApps
{
    public class App
    {
        [Key]
        public int Appid { get; set; }
        public string Name { get; set; }
    }
}