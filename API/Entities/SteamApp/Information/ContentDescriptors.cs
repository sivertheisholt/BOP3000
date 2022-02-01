using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace API.Entities.SteamApp.Information
{
    public class ContentDescriptors
    {
        public List<ContentDescriptorId> Ids { get; set; }
        public string Notes { get; set; }

        public AppData AppData { get; set; }
        public int AppDataId { get; set; }
    }
}