using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities.SteamApp.Information
{
    public class Movy
    {
        public int id { get; set; }
        public string name { get; set; }
        public string thumbnail { get; set; }
        public Webm webm { get; set; }
        public Mp4 mp4 { get; set; }
        public bool highlight { get; set; }
    }
}