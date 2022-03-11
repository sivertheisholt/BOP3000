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

        public Task<bool> AddPlayerToLobby(int userId)
        {
            throw new NotImplementedException();
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
            return await Context.Lobby.FindAsync(id);
        }
    }
}