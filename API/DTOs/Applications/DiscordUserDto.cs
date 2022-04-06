using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs.Applications
{
    public class DiscordUserDto
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Discriminator { get; set; }
        public string Avatar { get; set; }
        public bool Bot { get; set; }
        public bool System { get; set; }
        public bool Mfa_Enabled { get; set; }
        public string Banner { get; set; }
        public bool Verified { get; set; }
        public string Email { get; set; }
        public int Flags { get; set; }
        public int Premium_Type { get; set; }
        public int Public_Flags { get; set; }
    }
}