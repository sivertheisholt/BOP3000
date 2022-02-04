using API.Entities.GameRoom;

namespace API.Interfaces.IRepositories
{
    public interface ILobbiesRepository : IBaseRepository<GameRoom>
    {
        void addGameRoomAsync(GameRoom gameRoom);
    }
}