using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities.SteamApp;
using API.Interfaces.IRepositories;

namespace API.Data.Repositories
{
    public class SteamAppRepository : BaseRepository<GameInfo>, ISteamAppRepository
    {
        public SteamAppRepository(DataContext context) : base(context)
        {

        }

        public Task<GameInfo> GetGameInfo()
        {
            throw new NotImplementedException();
        }

        public void AddSteamApp(GameInfo gameInfo)
        {
            //Context.GameInfo.Add(gameInfo);
        }
    }
}