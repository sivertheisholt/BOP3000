using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities.SteamApps;

namespace API.Interfaces.IRepositories
{
    public interface ISteamAppsRepository : IBaseRepository<AppList>
    {
        Task<AppList> GetAppsInfoAsync(int id);

        void AddAppsList(AppList appList);

        Task<AppList> GetAppsList(int id);

        Task<List<AppList>> GetAllAppsList();
    }
}