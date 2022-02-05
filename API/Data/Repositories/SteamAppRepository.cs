using API.Entities.SteamApp;
using API.Interfaces.IRepositories;

namespace API.Data.Repositories
{
    public class SteamAppRepository : BaseRepository<AppInfo>, ISteamAppRepository
    {
        public SteamAppRepository(DataContext context) : base(context)
        {

        }

        public async Task<AppInfo> GetAppInfoAsync(int id)
        {
            return await Context.AppInfo.FindAsync(id);
        }

        public void AddApp(AppInfo appInfo)
        {
            Context.AppInfo.Add(appInfo);
        }
    }
}