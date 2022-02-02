using API.Entities.GameRoom;
using API.Interfaces.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repositories
{
    public class GameRoomRepository : BaseRepository<GameRoom>, IGameRoomRepository
    {
        public GameRoomRepository(DataContext context) : base(context)
        {
        }

        public void addGameRoomAsync(GameRoom gameRoom)
        {
            Context.GameRoom.Add(gameRoom);
        }
    }
}