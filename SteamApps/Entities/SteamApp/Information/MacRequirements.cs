using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace SteamApps.Entities.SteamApp.Information
{
    public class MacRequirements
    {
        [JsonProperty("minimum")]
        public string Minimum { get; set; }

        [JsonProperty("recommended")]
        public string Recommended { get; set; }
        public AppData AppData { get; set; }
        public int AppDataId { get; set; }
    }
}