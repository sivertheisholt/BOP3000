using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace API.Entities.SteamApp.Information
{
    public class Movy
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Thumbnail { get; set; }
        public Webm Webm { get; set; }
        public Mp4 Mp4 { get; set; }
        public bool Highlight { get; set; }
        public AppData AppData { get; set; }
        public int AppDataId { get; set; }
    }
}