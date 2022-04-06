using API.Entities.Applications;
using API.Entities.Users;

namespace API.Interfaces.IRepositories
{
    public interface IUserRepository : IBaseRepository<AppUser>
    {
        Task<IEnumerable<AppUser>> GetUsersAsync();
        Task<IEnumerable<AppUser>> GetUsersMeiliAsync();
        Task<AppUser> GetUserByUsernameAsync(string username);
        Task<AppUser> GetUserByIdAsync(int id);

        Task<AppUser> GetUserByEmailAsync(string email);

        Task<ICollection<int>> GetUserFollowers(int id);

        void UpdateUsername(AppUser user, string username);

        Task<bool> CheckIfUserExists(int id);

        Task<string> GetUsernameFromId(int id);

        void AddSteamId(AppUser user, long steamId);

        void AddDiscord(AppUser user, DiscordProfile discord);

        Task<string> GetUserDiscordAccessToken(int id);
        Task<ulong> GetUserDiscordIdFromUid(int id);

        Task<AppUserConnections> GetUserConnectionsFromUid(int id);
    }
}