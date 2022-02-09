using API.Entities.Users;

namespace API.Interfaces.IRepositories
{
    public interface IUserRepository : IBaseRepository<AppUser>
    {
        Task<IEnumerable<AppUser>> GetUsersAsync();
        Task<AppUser> GetUserByUsernameAsync(string username);
        Task<AppUser> GetUserByIdAsync(int id);
    }
}