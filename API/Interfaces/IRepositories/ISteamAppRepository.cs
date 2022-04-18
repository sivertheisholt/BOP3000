using API.Entities.SteamApp;
using API.Entities.SteamApp.Information;
using API.Helpers;
using API.Helpers.PaginationsParams;

namespace API.Interfaces.IRepositories
{
    public interface ISteamAppRepository : IBaseRepository<AppInfo>
    {
        Task<AppInfo> GetAppInfoAsync(int id);

        void AddApp(AppInfo appInfo);

        Task<PagedList<AppData>> GetActiveApps(UniversalParams universalParams);

        Task<PagedList<AppInfo>> GetAllApps(UniversalParams universalParams);

        Task<bool> CheckIfSaved(int gameId);
    }
}