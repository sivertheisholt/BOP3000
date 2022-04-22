using API.Entities.Lobbies;
using API.Entities.SteamApp;
using API.Entities.SteamApp.Information;
using API.Helpers;
using API.Helpers.PaginationsParams;
using API.Interfaces.IRepositories;
using Microsoft.EntityFrameworkCore;


namespace API.Data.Repositories
{
    public class SteamAppRepository : BaseRepository<AppInfo>, ISteamAppRepository
    {
        public SteamAppRepository(DataContext context) : base(context)
        {

        }

        public async Task<AppInfo> GetAppInfoAsync(int id)
        {
            return await Context.AppInfo.Include(test => test.Data)
                .FirstOrDefaultAsync(app => app.Id == id);
        }

        public void AddApp(AppInfo appInfo)
        {
            Context.AppInfo.Add(appInfo);
        }

        public async Task<PagedList<AppData>> GetActiveApps(UniversalParams universalParams)
        {
            var queryLobby = Context.Lobby.Where(lobby => !lobby.Finished)
                .Select(u => u.GameId)
                .Distinct()
                .AsQueryable();

            var query = Context.AppInfo.Where(app => queryLobby.Contains(app.Data.Id))
                .Select(app => app.Data)
                .AsNoTracking();

            return await PagedList<AppData>.CreateAsync(query, universalParams.PageNumber, universalParams.PageSize);
        }

        public async Task<PagedList<AppInfo>> GetAllApps(UniversalParams universalParams)
        {
            var query = Context.AppInfo;

            return await PagedList<AppInfo>.CreateAsync(query, universalParams.PageNumber, universalParams.PageSize);
        }

        public async Task<bool> CheckIfSaved(int gameId)
        {
            return await Context.AppInfo.Where(x => x.Data.SteamAppid == gameId).AnyAsync();
        }

        public async Task<AppInfo> GetAppInfoBySteamId(int id)
        {
            return await Context.AppInfo.Include(x => x.Data).FirstOrDefaultAsync(x => x.Data.SteamAppid == id);
        }
    }
}