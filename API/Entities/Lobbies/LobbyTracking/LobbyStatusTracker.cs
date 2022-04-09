using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities.Lobbies.LobbyTracking;

namespace API.Entities.Lobbies.LobbyTracking
{
    public class LobbyStatusTracker
    {
        public int LobbyId { get; set; }
        public int AdminUid { get; set; }
        public List<int> UsersQueue { get; set; }
        public List<LobbyUser> UsersLobby { get; set; }
        public List<int> BannedUsers { get; set; }
        public bool LobbyReadyCheck { get; set; }
        public bool Finished { get; set; }
    }
}