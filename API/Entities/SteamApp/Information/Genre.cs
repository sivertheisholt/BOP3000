using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace API.Entities.SteamApp.Information
{
    public class Genre
    {
        public string Id { get; set; }
        public string Description { get; set; }

        public AppData AppData { get; set; }
        public int AppDataId { get; set; }
    }
}