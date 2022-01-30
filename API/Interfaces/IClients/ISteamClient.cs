using API.Entities.SteamApps;

namespace API.Interfaces.IClients
{
    public interface ISteamClient
    {
        Task<Apps> GetAppsList();
    }
}