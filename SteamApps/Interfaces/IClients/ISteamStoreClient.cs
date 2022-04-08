using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SteamApps.Entities.SteamApp;

namespace SteamApps.Interfaces.IClients
{
    public interface ISteamStoreClient
    {
        Task<AppInfo> GetAppInfo(int appid);
    }
}