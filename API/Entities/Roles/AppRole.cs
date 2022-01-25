using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities.Users.Role;
using Microsoft.AspNetCore.Identity;

namespace API.Entities.Roles
{
    public class AppRole : IdentityRole<int>
    {
        public ICollection<AppUserRole> UserRoles { get; set; }

    }
}