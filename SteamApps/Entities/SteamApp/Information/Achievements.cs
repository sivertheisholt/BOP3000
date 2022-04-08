using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace SteamApps.Entities.SteamApp.Information
{
    public class Achievements
    {

        [JsonProperty("total")]
        public int Total { get; set; }

        [JsonProperty("highlighted")]
        public ICollection<Highlighted> Highlighted { get; set; }

        public AppData AppData { get; set; }
        public int AppDataId { get; set; }
    }
}