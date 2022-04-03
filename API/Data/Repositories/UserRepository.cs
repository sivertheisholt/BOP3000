using API.Entities.Applications;
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

        public void AddSteamId(AppUser user, long steamId)
        {
            user.AppUserProfile.UserConnections.SteamConnected = true;
            user.AppUserProfile.UserConnections.Steam.SteamId = steamId;
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
                .Include(p => p.AppUserProfile)
                .ThenInclude(p => p.UserConnections)
                .ThenInclude(p => p.Steam)
                .Include(p => p.AppUserProfile)
                .ThenInclude(p => p.AppUserPhoto)
                .FirstOrDefaultAsync();

            return user;
        }

        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
            return await Context.Users.SingleOrDefaultAsync(x => x.UserName == username);
        }

        public Task<ICollection<int>> GetUserFollowers(int id)
        {
            return Task.FromResult(Context.Users.Where(user => user.Id == id)
                    .Include(user => user.AppUserProfile)
                    .ThenInclude(user => user.AppUserData)
                    .Select(x => x.AppUserProfile.AppUserData.Followers).FirstOrDefault());
        }

        public async Task<string> GetUsernameFromId(int id)
        {
            return await Context.Users.Where(x => x.Id == id).Select(x => x.UserName).FirstOrDefaultAsync();
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

        public async Task<IEnumerable<AppUser>> GetUsersMeiliAsync()
        {
            return await Context.Users.ToListAsync();
        }

        public void UpdateUsername(AppUser user, string username)
        {
            user.UserName = username;
        }
    }
}