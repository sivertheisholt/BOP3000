using API.Entities.Users;
using API.Interfaces.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repositories
{
    public class UserRepository : BaseRepository<AppUser>, IUserRepository
    {
        public UserRepository(DataContext context) : base(context)
        {
        }

        public async Task<bool> CheckIfUserExists(int id)
        {
            return await Context.Users.AnyAsync(x => x.Id == id);
        }

        public async Task<AppUser> GetUserByEmailAsync(string email)
        {
            return await Context.Users.SingleOrDefaultAsync(x => x.Email == email);
        }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            var user = await Context.Users.Where(p => p.Id == id)
                .Include(p => p.AppUserProfile)
                .ThenInclude(p => p.AppUserData)
                .Include(p => p.AppUserProfile)
                .ThenInclude(p => p.CountryIso)
                .FirstOrDefaultAsync();

            return user;
        }

        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
            return await Context.Users.SingleOrDefaultAsync(x => x.UserName == username);
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await Context.Users
                .Include(p => p.AppUserProfile)
                .ThenInclude(p => p.AppUserData)
                .Include(p => p.AppUserProfile)
                .ThenInclude(p => p.CountryIso)
                .ToListAsync();
        }

        public void UpdateUsername(AppUser user, string username)
        {
            user.UserName = username;
        }
    }
}