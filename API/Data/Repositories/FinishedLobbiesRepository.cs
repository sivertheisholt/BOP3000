using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities.Lobbies;
using API.Interfaces.IRepositories;

namespace API.Data.Repositories
{
    public class FinishedLobbiesRepository : BaseRepository<FinishedLobby>, IFinishedLobbyRepository
    {
        public FinishedLobbiesRepository(DataContext context) : base(context)
        {
        }

        public void AddLobby(FinishedLobby lobby)
        {
            Context.FinishedLobby.Add(lobby);
        }
    }
}