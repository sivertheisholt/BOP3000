using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SteamApps.Entities.SteamApp;
using SteamApps.Entities.SteamApp.Information;
using SteamApps.Interfaces.IRepositories;

namespace SteamApps.Data.Repositories
{
    public class SteamAppRepository : BaseRepository<AppInfo>, ISteamAppRepository
    {
        public SteamAppRepository(DataContext context) : base(context)
        {

        }

        public async Task<AppInfo> GetAppInfoAsync(int id)
        {
            return await Context.AppInfo.Where(app => app.Id == id).Include(test => test.Data).FirstOrDefaultAsync();
        }

        public void AddApp(AppInfo appInfo)
        {
            Context.AppInfo.Add(appInfo);
        }
    }
}