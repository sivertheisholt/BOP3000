using API.Entities.SteamApps;

namespace API.Interfaces.IClients
{
    public interface ISteamAppsClient
    {
        Task<Apps> GetAppsList();
    }
}