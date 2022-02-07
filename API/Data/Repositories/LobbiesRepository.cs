using API.Entities.Lobbies;
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

        public Task<List<Lobby>> GetLobbiesAsync()
        {
            //var query = Context.Lobby.Where(x => x.GameType == "Casual").OrderBy(X => X.Id).Take(5).Skip(5).AsQueryable();
            return Task.FromResult(Context.Lobby.ToList());
        }

        public async Task<Lobby> GetLobbyAsync(int id)
        {
            return await Context.Lobby.FindAsync(id);
        }
    }
}