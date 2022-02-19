using API.Entities.SteamApp;
using API.Entities.SteamApp.Information;

namespace API.Interfaces.IRepositories
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISteamAppRepository : IBaseRepository<AppInfo>
    {
        Task<AppInfo> GetAppInfoAsync(int id);

        void AddApp(AppInfo appInfo);

        Task<List<AppData>> GetActiveApps();
    }
}