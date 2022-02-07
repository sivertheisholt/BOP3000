using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities.Lobbies
{
    public class LobbiesList
    {
        public ICollection<Lobby> Lobbies { get; set; }
    }
}