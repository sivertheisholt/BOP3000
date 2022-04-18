using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities.SteamApps;
using API.Interfaces.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repositories
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

        public async Task<List<AppList>> GetAllAppsList()
        {
            return await Context.AppList.ToListAsync();
        }

        public async Task<AppList> GetAppsInfoAsync(int id)
        {
            return await Context.AppList.FindAsync(id);
        }

        public async Task<AppList> GetAppsList(int id)
        {
            return await Context.AppList.Include(app => app.Apps)
                .FirstOrDefaultAsync(app => app.Id == id);
        }
    }
}