using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities.SteamApp.Information
{
    public class Screenshot
    {
        public int id { get; set; }
        public string path_thumbnail { get; set; }
        public string path_full { get; set; }
    }
}