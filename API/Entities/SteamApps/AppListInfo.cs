using Newtonsoft.Json;
using basicJsonIgnore = System.Text.Json.Serialization;

namespace API.Entities.SteamApps
{
    public class AppListInfo
    {
        public int AppListInfoId { get; set; }

        [JsonProperty("appid")]
        public int AppId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [basicJsonIgnore.JsonIgnore]
        public AppList AppList { get; set; }

        [basicJsonIgnore.JsonIgnore]
        public int AppListId { get; set; }
    }
}