using API.Entities.GameRoom;

namespace API.Interfaces.IRepositories
{
    public interface IGameRoomRepository
    {
        void addGameRoomAsync(GameRoom gameRoom);
        Task<bool> SaveAllAsync();
    }
}