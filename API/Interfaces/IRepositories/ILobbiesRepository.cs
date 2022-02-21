using API.Entities.Lobbies;
using API.Entities.SteamApp;

namespace API.Interfaces.IRepositories
{
    public interface ILobbiesRepository : IBaseRepository<Lobby>
    {
        void AddLobby(Lobby loby);

        Task<Lobby> GetLobbyAsync(int id);

        Task<List<Lobby>> GetLobbiesAsync();

        Task<bool> AddPlayerToLobby(int userId);

        Task<List<Lobby>> GetLobbiesWithGameId(int id);

    }
}