using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SteamApps.Entities.SteamApps;

namespace SteamApps.Interfaces.IRepositories
{
    public interface ISteamAppsRepository : IBaseRepository<AppList>
    {
        Task<AppList> GetAppsInfoAsync(int id);

        void AddAppsList(AppList appList);

        Task<AppList> GetAppsList(int id);
    }
}