using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SteamApps.Entities.SteamApp;
using SteamApps.Entities.SteamApp.Information;

namespace SteamApps.Interfaces.IRepositories
{
    public interface ISteamAppRepository : IBaseRepository<AppInfo>
    {
        Task<AppInfo> GetAppInfoAsync(int id);

        void AddApp(AppInfo appInfo);
    }
}