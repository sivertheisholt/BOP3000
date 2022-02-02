using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace API.Entities.SteamApp.Information
{
    public class Recommendations
    {

        [JsonProperty("total")]
        public uint Total { get; set; }

        public AppData AppData { get; set; }
        public int AppDataId { get; set; }
    }
}