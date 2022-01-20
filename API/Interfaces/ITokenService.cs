using API.Entities;
using API.Entities.Users;

namespace API.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);

    }
}