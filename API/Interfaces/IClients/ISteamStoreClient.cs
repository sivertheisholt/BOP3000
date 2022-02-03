using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities.SteamApp;

namespace API.Interfaces.IClients
{
    public interface ISteamStoreClient
    {
        Task<GameInfo> GetAppInfo(int appid);
    }
}