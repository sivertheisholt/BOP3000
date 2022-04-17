using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities.SteamApps
{
    public class AppListHistory
    {
        public bool Current { get; set; }
        public int Size { get; set; }
        public AppList AppList { get; set; }
        public int AppListId { get; set; }
    }
}