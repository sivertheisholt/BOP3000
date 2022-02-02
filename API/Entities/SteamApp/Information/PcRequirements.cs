using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace API.Entities.SteamApp.Information
{
    public class PcRequirements
    {
        public string Minimum { get; set; }
        public string Recommended { get; set; }
        public AppData AppData { get; set; }
        public int AppDataId { get; set; }
    }
}