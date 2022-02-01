using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities.SteamApp.Information
{
    public class Developer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public AppData AppData { get; set; }
        public int AppDataId { get; set; }
    }
}