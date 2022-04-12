using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs.Applications
{
    public class DiscordTokenDto
    {
        public string Access_Token { get; set; }
        public int Expires_In { get; set; }
        public int Refresh_Token { get; set; }
    }
}