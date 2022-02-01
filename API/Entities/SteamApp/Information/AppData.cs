using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities.SteamApp.Information
{
    public class AppData
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public int Steam_appid { get; set; }
        public int Required_age { get; set; }
        public bool Is_free { get; set; }
        public string Controller_support { get; set; }

        public string Detailed_description { get; set; }
        public string About_the_game { get; set; }
        public string Short_description { get; set; }
        public string Supported_languages { get; set; }
        public string Header_image { get; set; }
        public string Website { get; set; }
        public string Legal_notice { get; set; }
        public string Background { get; set; }
        public List<Dlc> Dlc { get; set; }
        public List<Developer> Developers { get; set; }
        public List<Publisher> Publishers { get; set; }
        public List<PackageGroup> Package_groups { get; set; }
        public Platforms Platforms { get; set; }
        public Metacritic Metacritic { get; set; }
        public ICollection<Category> Categories { get; set; }
        public ICollection<Genre> Genres { get; set; }
        public ICollection<Screenshot> Screenshots { get; set; }
        public ICollection<Movy> Movies { get; set; }
        public Achievements Achievements { get; set; }
        public ReleaseDate Release_date { get; set; }
        public SupportInfo Support_info { get; set; }
        public ContentDescriptors Content_descriptors { get; set; }
        public PcRequirements Pc_requirements { get; set; }
        public MacRequirements Mac_requirements { get; set; }
        public LinuxRequirements Linux_requirements { get; set; }
        public GameInfo GameInfo { get; set; }
        public int GameInfoId { get; set; }
    }
}