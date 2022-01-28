using API.Entities.GameRoom;
using API.Interfaces.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repositories
{
    public class GameRoomRepository : IGameRoomRepository
    {

        private readonly DataContext _context;
        public GameRoomRepository(DataContext context)
        {
            _context = context;
        }

        public void addGameRoomAsync(GameRoom gameRoom)
        {
            _context.GameRoom.Add(gameRoom);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update(GameRoom gameRoom)
        {
            _context.Entry(gameRoom).State = EntityState.Modified;
        }

    }
}