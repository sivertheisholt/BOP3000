using API.Entities.Lobbies;
using API.Entities.SteamApp;
using API.Entities.SteamApp.Information;
using API.Interfaces.IRepositories;
using Microsoft.EntityFrameworkCore;


namespace API.Data.Repositories
{
    public class SteamAppRepository : BaseRepository<AppInfo>, ISteamAppRepository
    {
        public SteamAppRepository(DataContext context) : base(context)
        {

        }

        public Task<AppInfo> GetAppInfoAsync(int id)
        {
            var app = Context.AppInfo.Where(app => app.Id == id).Include(test => test.Data).First();
            return Task.FromResult(app);
        }

        public void AddApp(AppInfo appInfo)
        {
            Context.AppInfo.Add(appInfo);
        }

        public Task<List<AppData>> GetActiveApps()
        {
            var queryLobby = Context.Lobby.Select(u => u.GameId).Distinct().AsQueryable();

            var apps = Context.AppInfo.Where(app => queryLobby.Contains(app.Data.SteamAppid)).Select(app => app.Data).ToList();
            return Task.FromResult(apps);
        }
    }
}