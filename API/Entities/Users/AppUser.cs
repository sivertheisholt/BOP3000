using API.Entities.Users.Role;
using Microsoft.AspNetCore.Identity;

namespace API.Entities.Users
{
    public class AppUser : IdentityUser<int>
    {
        public ICollection<AppUserRole> UserRoles { get; set; }
        public AppUserProfile AppUserProfile { get; set; }

    }
}