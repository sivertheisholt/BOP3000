using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs.Applications
{
    public class DiscordTokenDto
    {
        public string AccessToken { get; set; }
        public int ExpiresIn { get; set; }
        public int MyProperty { get; set; }
    }
}