using API.Entities.Lobbies;
using API.Interfaces.IRepositories;
using Microsoft.EntityFrameworkCore;

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

        public Task<int> CountLobbiesWithGameId(int id)
        {
            return Task.FromResult(Context.Lobby.Where(o => o.GameId == id).Count());
        }

        public async Task<List<Lobby>> GetActiveLobbies()
        {
            return await Context.Lobby.Where(x => !x.Finished).ToListAsync();
        }

        public Task<List<Lobby>> GetLobbiesAsync()
        {
            //var query = Context.Lobby.Where(x => x.GameType == "Casual").OrderBy(X => X.Id).Take(5).Skip(5).AsQueryable();
            return Task.FromResult(Context.Lobby.ToList());
        }

        public Task<List<Lobby>> GetLobbiesWithGameId(int id)
        {
            var lobbies = Context.Lobby.Where(lobby => lobby.GameId == id)
            .Include(lobby => lobby.LobbyRequirement)
            .ToList();
            return Task.FromResult(lobbies);
        }

        public async Task<Lobby> GetLobbyAsync(int id)
        {
            return await Context.Lobby.Where(lobby => lobby.Id == id)
                .Include(lobby => lobby.Votes).FirstOrDefaultAsync();
        }
    }
}