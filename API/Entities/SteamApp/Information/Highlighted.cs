using Newtonsoft.Json;

namespace API.Entities.SteamApp.Information
{
    public class Highlighted
    {
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("path")]
        public string Path { get; set; }

        public Achievements Achievements { get; set; }
        public int AchievementsId { get; set; }
    }
}