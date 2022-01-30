using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities.SteamGame
{
    public class Achievements
    {
        public int total { get; set; }
        public List<Highlighted> highlighted { get; set; }
    }
}