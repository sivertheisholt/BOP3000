using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities.Lobbies.LobbyTracking
{
    public class LobbyChatStatusTracker
    {
        public int LobbyId { get; set; }
        public List<Message> Messages { get; set; }
        public List<int> Users { get; set; }
    }
}