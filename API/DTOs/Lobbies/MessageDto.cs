using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs.Lobbies
{
    public class MessageDto
    {
        public int Uid { get; set; }
        public DateTime DateSent { get; set; }

        public string ChatMessage { get; set; }
    }
}