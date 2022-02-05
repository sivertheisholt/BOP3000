using API.Entities.Lobby;
using API.Entities.SteamApp;

namespace API.Interfaces.IRepositories
{
    public interface ILobbiesRepository : IBaseRepository<Lobby>
    {
        void AddLobby(Lobby loby);

        Task<Lobby> GetLobbyAsync(int id);
    }
}