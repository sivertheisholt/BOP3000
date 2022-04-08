using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SteamApps.Entities.SteamApps;
using SteamApps.Interfaces.IRepositories;

namespace SteamApps.Data.Repositories
{
    public class SteamAppsRepository : BaseRepository<AppList>, ISteamAppsRepository
    {
        public SteamAppsRepository(DataContext context) : base(context)
        {
        }

        public void AddAppsList(AppList appList)
        {
            Context.AppList.Add(appList);
        }

        public async Task<AppList> GetAppsInfoAsync(int id)
        {
            return await Context.AppList.FindAsync(id);
        }

        public async Task<AppList> GetAppsList(int id)
        {
            return await Context.AppList.Where(app => app.Id == id)
                .Include(app => app.Apps)
                .FirstOrDefaultAsync();
        }
    }
}