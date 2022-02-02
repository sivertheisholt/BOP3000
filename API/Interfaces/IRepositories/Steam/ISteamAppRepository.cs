using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities.SteamApp;

namespace API.Interfaces.IRepositories
{
    public interface ISteamAppRepository : IBaseRepository<GameInfo>
    {
        Task<GameInfo> GetGameInfoAsync(int id);

        void AddSteamApp(GameInfo gameInfo);
    }
}