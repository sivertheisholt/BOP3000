using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs.Lobbies
{
    public class LobbyVoteDto
    {
        public int VoterUid { get; set; }
        public int VotedUid { get; set; }
        public bool Upvote { get; set; }
    }
}