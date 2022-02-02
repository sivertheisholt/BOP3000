using API.Entities.SteamApps;
using API.Interfaces.IClients;
using API.Interfaces.IRepositories.Steam;

namespace API.Data.Repositories
{
    public class SteamAppsRepository : BaseRepository<Apps>, ISteamAppsRepository
    {
        private readonly ISteamAppsClient _steamAppsClient;
        public SteamAppsRepository(DataContext context, ISteamAppsClient steamAppsClient) : base(context)
        {
            _steamAppsClient = steamAppsClient;
        }

        public async Task<Apps> GetAppsAsync()
        {
            return await _steamAppsClient.GetAppsList();
        }
    }
}