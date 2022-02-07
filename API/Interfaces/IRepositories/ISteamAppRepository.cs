using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities.SteamApp;

namespace API.Interfaces.IRepositories.Apps
{
    public interface ISteamAppRepository : IBaseRepository<AppInfo>
    {
        Task<AppInfo> GetAppInfoAsync(int id);

        void AddApp(AppInfo appInfo);
    }
}