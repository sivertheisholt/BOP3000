using API.Entities.Lobbies;
using API.Entities.SteamApp;

namespace API.Interfaces.IRepositories
{
    public interface ILobbiesRepository : IBaseRepository<Lobby>
    {
        void AddLobby(Lobby loby);

        Task<Lobby> GetLobbyAsync(int id);

        Task<List<Lobby>> GetLobbiesAsync();

        Task<List<Lobby>> GetLobbiesWithGameId(int id);

        Task<int> CountLobbiesWithGameId(int id);

        Task<List<Lobby>> GetActiveLobbies();

    }
}