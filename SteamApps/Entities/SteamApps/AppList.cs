using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace SteamApps.Entities.SteamApps
{
    public class AppList
    {
        [Key]
        public int Id { get; set; }

        [JsonProperty("apps")]
        public ICollection<AppListInfo> Apps { get; set; }

        [JsonIgnore]
        public bool Success { get; set; }
    }
}