using System.ComponentModel.DataAnnotations;
using SteamApps.Entities.SteamApp.Information;

namespace SteamApps.Entities.SteamApp
{
    public class AppInfo
    {
        [Key]
        public int Id { get; set; }
        public bool Success { get; set; }
        public AppData Data { get; set; }
    }
}