using SteamApps.Entities.SteamApps;

namespace SteamApps.Interfaces.IClients
{
    public interface ISteamAppsClient
    {
        Task<AppList> GetAppsList();
    }
}