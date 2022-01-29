using API.Entities.GameRoom;

namespace API.Interfaces.IRepositories
{
    public interface IGameRoomRepository : IBaseRepository<GameRoom>
    {
        void addGameRoomAsync(GameRoom gameRoom);
    }
}