using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace API.Entities.SteamApp.Information
{
    public class Highlighted
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }

        public Achievements Achievements { get; set; }
        public int AchievementsId { get; set; }
    }
}