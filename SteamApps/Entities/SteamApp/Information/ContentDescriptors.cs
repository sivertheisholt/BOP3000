using Newtonsoft.Json;

namespace SteamApps.Entities.SteamApp.Information
{
    public class ContentDescriptors
    {

        [JsonProperty("ids")]
        public ICollection<string> Ids { get; set; }

        [JsonProperty("notes")]
        public string Notes { get; set; }

        public AppData AppData { get; set; }
        public int AppDataId { get; set; }
    }
}