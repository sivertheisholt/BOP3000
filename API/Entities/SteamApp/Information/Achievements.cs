using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace API.Entities.SteamApp.Information
{
    public class Achievements
    {
        public int Id { get; set; }
        public int Total { get; set; }
        public ICollection<Highlighted> Highlighted { get; set; }

        public AppData AppData { get; set; }
        public int AppDataId { get; set; }
    }
}