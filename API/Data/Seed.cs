using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities.Roles;
using API.Entities.Users;
using API.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class Seed
    {
        public static async Task SeedUsers(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            if (await userManager.Users.AnyAsync()) return;

            var roles = new List<AppRole>
            {
                new AppRole { Name = Role.Member.MakeString() },
                new AppRole { Name = Role.Admin.MakeString() },
                new AppRole { Name = Role.Premium.MakeString() }
            };

            foreach (var role in roles)
            {
                await roleManager.CreateAsync(role);
            }

            var admin = new AppUser
            {
                UserName = "admin"
            };

            var testUser = new AppUser
            {
                UserName = "testbruker"
            };

            await userManager.CreateAsync(admin, "Pa$$w0rd");
            await userManager.AddToRolesAsync(admin, new[] { "Admin", "Premium" });

            await userManager.CreateAsync(testUser, "Playfu123!");
            await userManager.AddToRolesAsync(admin, new[] { Role.Member.MakeString() });

        }
    }
}