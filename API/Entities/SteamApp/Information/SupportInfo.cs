using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace API.Entities.SteamApp.Information
{
    public class SupportInfo
    {
        public string Url { get; set; }
        public string Email { get; set; }

        public AppData AppData { get; set; }
        public int AppDataId { get; set; }
    }
}