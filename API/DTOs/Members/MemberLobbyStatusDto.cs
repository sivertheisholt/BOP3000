using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs.Members
{
    public class MemberLobbyStatusDto
    {
        public bool InQueue { get; set; }
        public int LobbyId { get; set; }
    }
}