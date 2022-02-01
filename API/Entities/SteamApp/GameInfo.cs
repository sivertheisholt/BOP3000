using System.ComponentModel.DataAnnotations;
using API.Entities.SteamApp.Information;

namespace API.Entities.SteamApp
{
    public class GameInfo
    {
        [Key]
        public int Id { get; set; }
        public bool Success { get; set; }
        public AppData AppData { get; set; }
    }
}