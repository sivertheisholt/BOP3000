using Newtonsoft.Json;

namespace API.Entities.SteamApp.Information
{
    public class Genre
    {
        public int GenreId { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        public AppData AppData { get; set; }
        public int AppDataId { get; set; }
    }
}