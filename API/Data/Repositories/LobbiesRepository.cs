using API.Entities.Lobbies;
using API.Helpers;
using API.Helpers.PaginationsParams;
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

        public async Task<int> CountActiveLobbiesWithGameId(int id)
        {
            return await Context.Lobby.Where(o => o.GameId == id)
                .Where(o => !o.Finished)
                .CountAsync();
        }

        public async Task<PagedList<Lobby>> GetActiveLobbies(UniversalParams universalParams)
        {
            var query = Context.Lobby.Where(x => !x.Finished)
                .AsNoTracking();
            return await PagedList<Lobby>.CreateAsync(query, universalParams.PageNumber, universalParams.PageSize);
        }

        public async Task<List<Lobby>> GetActiveRecommendedLobbies(int amount)
        {
            return await Context.Lobby.Where(x => !x.Finished).OrderBy(x => Guid.NewGuid()).Take(amount).ToListAsync();
        }

        public async Task<List<Lobby>> GetAllLobbiesNoPaging()
        {
            return await Context.Lobby.ToListAsync();
        }

        public async Task<PagedList<Lobby>> GetLobbiesAsync(UniversalParams universalParams)
        {
            var query = Context.Lobby.AsNoTracking();
            return await PagedList<Lobby>.CreateAsync(query, universalParams.PageNumber, universalParams.PageSize);
        }

        public async Task<PagedList<Lobby>> GetLobbiesWithGameId(int id, UniversalParams universalParams)
        {
            var query = Context.Lobby.Where(lobby => lobby.GameId == id)
            .Include(lobby => lobby.LobbyRequirement);
            return await PagedList<Lobby>.CreateAsync(query, universalParams.PageNumber, universalParams.PageSize);
        }

        public async Task<Lobby> GetLobbyAsync(int id)
        {
            return await Context.Lobby.Include(lobby => lobby.Votes)
                .Include(lobby => lobby.Log)
                .ThenInclude(log => log.Messages)
                .FirstOrDefaultAsync(lobby => lobby.Id == id);
        }
    }
}