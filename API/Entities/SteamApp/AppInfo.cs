using System.ComponentModel.DataAnnotations;
using API.Entities.SteamApp.Information;

namespace API.Entities.SteamApp
{
    public class AppInfo
    {
        [Key]
        public int Id { get; set; }
        public bool Success { get; set; }
        public AppData Data { get; set; }
    }
}