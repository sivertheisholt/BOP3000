using API.Entities.SteamApp;
using API.Interfaces.IRepositories;

namespace API.Data.Repositories
{
    public class SteamAppRepository : BaseRepository<GameInfo>, ISteamAppRepository
    {
        public SteamAppRepository(DataContext context) : base(context)
        {

        }

        public async Task<GameInfo> GetGameInfoAsync(int id)
        {
            return await Context.GameInfo.FindAsync(id);
        }

        public void AddSteamApp(GameInfo gameInfo)
        {
            Context.GameInfo.Add(gameInfo);
        }
    }
}