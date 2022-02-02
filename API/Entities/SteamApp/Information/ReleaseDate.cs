using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace API.Entities.SteamApp.Information
{
    public class ReleaseDate
    {
        public bool Coming_soon { get; set; }
        public string Date { get; set; }

        public AppData AppData { get; set; }
        public int AppDataId { get; set; }
    }
}