using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities.SteamApp;

namespace API.Interfaces.IRepositories
{
    public interface ISteamAppRepository
    {
        Task<GameInfo> GetGameInfo();
    }
}