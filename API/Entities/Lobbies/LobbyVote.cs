using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities.Lobbies
{
    public class LobbyVote
    {

        public int LobbyVoteId { get; set; }
        public int VoterUid { get; set; }
        public int VotedUid { get; set; }
        public bool Upvote { get; set; }

        public Lobby Lobby { get; set; }
        public int LobbyId { get; set; }
    }
}