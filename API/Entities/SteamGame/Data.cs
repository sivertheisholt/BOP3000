using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities.SteamGame
{
    public class Data
    {
        public string type { get; set; }
        public string name { get; set; }
        public int steam_appid { get; set; }
        public int required_age { get; set; }
        public bool is_free { get; set; }
        public string controller_support { get; set; }
        public List<int> dlc { get; set; }
        public string detailed_description { get; set; }
        public string about_the_game { get; set; }
        public string short_description { get; set; }
        public string supported_languages { get; set; }
        public string header_image { get; set; }
        public string website { get; set; }
        public PcRequirements pc_requirements { get; set; }
        public MacRequirements mac_requirements { get; set; }
        public LinuxRequirements linux_requirements { get; set; }
        public string legal_notice { get; set; }
        public List<string> developers { get; set; }
        public List<string> publishers { get; set; }
        public List<object> package_groups { get; set; }
        public Platforms platforms { get; set; }
        public Metacritic metacritic { get; set; }
        public List<Category> categories { get; set; }
        public List<Genre> genres { get; set; }
        public List<Screenshot> screenshots { get; set; }
        public List<Movy> movies { get; set; }
        public Achievements achievements { get; set; }
        public ReleaseDate release_date { get; set; }
        public SupportInfo support_info { get; set; }
        public string background { get; set; }
        public ContentDescriptors content_descriptors { get; set; }
    }
}