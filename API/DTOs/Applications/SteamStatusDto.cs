using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs.Applications
{
    public class SteamStatusDto
    {
        public bool Connected { get; set; }
        public long SteamId { get; set; }
        public bool Hidden { get; set; }
    }
}