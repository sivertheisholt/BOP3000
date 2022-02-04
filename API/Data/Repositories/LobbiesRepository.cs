using API.Entities.GameRoom;
using API.Interfaces.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repositories
{
    public class LobbiesRepository : BaseRepository<GameRoom>, ILobbiesRepository
    {
        public LobbiesRepository(DataContext context) : base(context)
        {
        }

        public void addGameRoomAsync(GameRoom gameRoom)
        {
            Context.GameRoom.Add(gameRoom);
        }
    }
}