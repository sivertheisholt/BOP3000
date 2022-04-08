using Newtonsoft.Json;

namespace SteamApps.Entities.SteamApp.Information
{
    public class LinuxRequirements
    {

        [JsonProperty("minimum")]
        public string Minimum { get; set; }

        [JsonProperty("recommended")]
        public string Recommended { get; set; }

        public AppData AppData { get; set; }
        public int AppDataId { get; set; }
    }
}