using API.Entities;
using API.Entities.Users;

namespace API.Interfaces.IServices
{
    public interface ITokenService
    {
        Task<string> CreateToken(AppUser user);

    }
}