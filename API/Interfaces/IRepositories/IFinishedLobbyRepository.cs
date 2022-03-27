using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities.Lobbies;

namespace API.Interfaces.IRepositories
{
    public interface IFinishedLobbyRepository : IBaseRepository<FinishedLobby>
    {
        void AddLobby(FinishedLobby lobby);
    }
}