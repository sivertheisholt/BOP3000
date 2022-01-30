using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities.SteamGame;

namespace API.Interfaces.IClients
{
    public interface ISteamStoreClient
    {
        Task<GameInfo> GetAppInfo(string appid);
    }
}