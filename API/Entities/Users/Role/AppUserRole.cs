using API.Entities.Roles;
using Microsoft.AspNetCore.Identity;

namespace API.Entities.Users.Role
{
    public class AppUserRole : IdentityUserRole<int>
    {
        public AppUser User { get; set; }
        public AppRole Role { get; set; }

    }
}