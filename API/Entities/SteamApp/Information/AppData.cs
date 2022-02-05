using System.Text.Json;
using Newtonsoft.Json;

namespace API.Entities.SteamApp.Information
{
    public class AppData
    {
        public int Id { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("steam_appid")]
        public int SteamAppid { get; set; }

        [JsonProperty("required_age")]
        public int RequiredAge { get; set; }

        [JsonProperty("is_free")]
        public bool IsFree { get; set; }

        [JsonProperty("controller_support")]
        public string ControllerSupport { get; set; }

        [JsonProperty("detailed_description")]
        public string DetailedDescription { get; set; }

        [JsonProperty("about_the_game")]
        public string AboutTheGame { get; set; }

        [JsonProperty("short_description")]
        public string ShortDescription { get; set; }

        [JsonProperty("supported_languages")]
        public string SupportedLanguages { get; set; }

        [JsonProperty("header_image")]
        public string HeaderImage { get; set; }

        [JsonProperty("website")]
        public string Website { get; set; }

        [JsonProperty("legal_notice")]
        public string LegalNotice { get; set; }

        [JsonProperty("background")]
        public string Background { get; set; }

        [JsonProperty("dlc")]
        public ICollection<int> Dlc { get; set; }

        [JsonProperty("developers")]
        public ICollection<string> Developers { get; set; }

        [JsonProperty("publishers")]
        public ICollection<string> Publishers { get; set; }

        [JsonProperty("package_groups")]
        public ICollection<PackageGroup> PackageGroups { get; set; }

        [JsonProperty("platforms")]
        public Platforms Platforms { get; set; }

        [JsonProperty("metacritic")]
        public Metacritic Metacritic { get; set; }

        [JsonProperty("categories")]
        public ICollection<Category> Categories { get; set; }

        [JsonProperty("genres")]
        public ICollection<Genre> Genres { get; set; }

        [JsonProperty("screenshots")]
        public ICollection<Screenshot> Screenshots { get; set; }

        [JsonProperty("movies")]
        public ICollection<Movy> Movies { get; set; }

        [JsonProperty("achievements")]
        public Achievements Achievements { get; set; }

        [JsonProperty("release_date")]
        public ReleaseDate ReleaseDate { get; set; }

        [JsonProperty("support_info")]
        public SupportInfo SupportInfo { get; set; }

        [JsonProperty("content_descriptors")]
        public ContentDescriptors ContentDescriptors { get; set; }

        [JsonProperty("pc_requirements")]
        public PcRequirements PcRequirements { get; set; }

        [JsonProperty("mac_requirements")]
        public MacRequirements MacRequirements { get; set; }

        [JsonProperty("linux_requirements")]
        public LinuxRequirements LinuxRequirements { get; set; }

        [JsonProperty("price_overview")]
        public Price PriceOverview { get; set; }

        [JsonProperty("recommendations")]
        public Recommendations Recommendations { get; set; }

        public AppInfo AppInfo { get; set; }
        public int AppInfoId { get; set; }
    }
}