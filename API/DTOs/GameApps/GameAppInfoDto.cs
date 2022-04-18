using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs.GameApps
{
    public class GameAppInfoDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string HeaderImage { get; set; }
        public string Background { get; set; }
        public int ActiveLobbies { get; set; }
        public int SteamAppid { get; set; }
    }
}