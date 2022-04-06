using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs.Applications
{
    public class DiscordStatusDto
    {
        public bool Connected { get; set; }
        public string Username { get; set; }
        public string Discriminator { get; set; }
        public bool Hidden { get; set; }
    }
}