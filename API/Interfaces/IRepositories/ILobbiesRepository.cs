using API.Entities.Lobbies;
using API.Entities.SteamApp;
using API.Helpers;
using API.Helpers.PaginationsParams;

namespace API.Interfaces.IRepositories
{
    public interface ILobbiesRepository : IBaseRepository<Lobby>
    {
        void AddLobby(Lobby loby);

        Task<Lobby> GetLobbyAsync(int id);

        Task<PagedList<Lobby>> GetLobbiesAsync(UniversalParams universalParams);

        Task<PagedList<Lobby>> GetLobbiesWithGameId(int id, UniversalParams universalParams);

        Task<int> CountActiveLobbiesWithGameId(int id);

        Task<PagedList<Lobby>> GetActiveLobbies(UniversalParams universalParams);

    }
}