using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace API.Entities.SteamApp.Information
{
    public class Webm
    {
        [JsonProperty("480")]
        public string Resolution480 { get; set; }

        [JsonProperty("max")]
        public string Max { get; set; }
        public Movy Movy { get; set; }
        public int MovyId { get; set; }
    }
}