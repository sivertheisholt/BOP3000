using API.Entities.Roles;
using API.Entities.SteamApps;
using API.Entities.Users;
using API.Enums;
using API.Interfaces.IClients;
using API.Interfaces.IRepositories;
using API.Interfaces.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class Seed
    {
        /// <summary>
        /// Will seed a few test users to the database for development
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="roleManager"></param>
        /// <returns></returns>
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
                UserName = "adminTest",
                Email = "admin@test.com"
            };

            var testUser = new AppUser
            {
                UserName = "membertest",
                Email = "member@test.com"
            };

            await userManager.CreateAsync(admin, "Pa$$w0rd");
            await userManager.AddToRolesAsync(admin, new[] { "Admin", "Premium" });

            await userManager.CreateAsync(testUser, "Playfu123!");
            await userManager.AddToRolesAsync(admin, new[] { Role.Member.MakeString() });
        }

        /// <summary>
        /// Will seed 50 games to the database for development and add all the apps to the database. 
        /// </summary>
        /// <param name="steamAppRepository"></param>
        /// <param name="steamStoreClient"></param>
        /// <param name="steamAppsClient"></param>
        /// <returns></returns>
        public static async Task SeedSteamApps(ISteamAppRepository steamAppRepository, ISteamAppsRepository steamAppsRepository,
                                                    ISteamStoreClient steamStoreClient, ISteamAppsClient steamAppsClient,
                                                        IMeilisearchService meilisearchService)
        {
            var max = 50;
            var counter = 0;

            var apps = await steamAppsClient.GetAppsList();


            if (await steamAppRepository.GetAppInfoAsync(1) != null) return;

            //var apps = await steamAppsClient.GetAppsList();
            steamAppsRepository.AddAppsList(apps);

            foreach (AppListInfo app in apps.Apps)
            {
                if (counter >= max) break;

                var gameResult = await steamStoreClient.GetAppInfo(app.AppId);
                if (!gameResult.Success) continue;

                steamAppRepository.AddApp(gameResult);

                counter++;
            }

            await steamAppRepository.SaveAllAsync();
            await steamAppsRepository.SaveAllAsync();

            //Seed to search
            var dbResult = await steamAppsRepository.GetAppsList(1);
            await meilisearchService.initializeIndexAsync(dbResult);

            Console.WriteLine($"Finished seeding Steam data");
        }
    }
}