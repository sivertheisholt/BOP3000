using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities.Lobbies
{
    public class Message
    {
        public int Id { get; set; }
        public int LobbyId { get; set; }
        public int Uid { get; set; }
        public DateTime DateSent { get; set; }
    }
}