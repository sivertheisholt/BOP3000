using API.Entities.Roles;
using API.Entities.SteamApps;
using API.Entities.Users;
using API.Enums;
using API.Interfaces.IClients;
using API.Interfaces.IRepositories.Apps;
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

        public static async Task SeedSteamGames(ISteamAppRepository steamAppRepository, ISteamStoreClient steamStoreClient, ISteamAppsClient steamAppsClient)
        {
            var max = 50;
            var counter = 0;

            if (await steamAppRepository.GetAppInfoAsync(1) != null) return;

            var apps = await steamAppsClient.GetAppsList();

            foreach (App app in apps.Applist.Apps)
            {
                if (counter >= max) break;

                var gameResult = await steamStoreClient.GetAppInfo(app.Appid);
                if (!gameResult.Success) continue;

                steamAppRepository.AddApp(gameResult);

                counter++;
            }

            var result = await steamAppRepository.SaveAllAsync();
            Console.WriteLine($"Finished seeding Steam data");
        }
    }
}