using API.Entities.Lobby;
using API.Interfaces.IRepositories;

namespace API.Data.Repositories
{
    public class LobbiesRepository : BaseRepository<Lobby>, ILobbiesRepository
    {
        public LobbiesRepository(DataContext context) : base(context)
        {
        }

        public void AddLobby(Lobby lobby)
        {
            Context.Lobby.Add(lobby);
        }

        public async Task<Lobby> GetLobbyAsync(int id)
        {
            return await Context.Lobby.FindAsync(id);
        }
    }
}