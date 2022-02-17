using API.Entities.Countries;
using API.Entities.Roles;
using API.Entities.SteamApps;
using API.Entities.Users;
using API.Enums;
using API.Interfaces.IClients;
using API.Interfaces.IRepositories;
using API.Interfaces.IServices;
using ISO3166;
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
                Email = "admin@test.com",
                AppUserProfile = new AppUserProfile
                {
                    Birthday = new DateTime(1998, 7, 30),
                    Gender = "Male",
                    CountryIso = new CountryIso
                    {
                        Name = ISO3166.Country.List.First().Name,
                        TwoLetterCode = ISO3166.Country.List.First().TwoLetterCode,
                        ThreeLetterCode = ISO3166.Country.List.First().ThreeLetterCode,
                        NumericCode = ISO3166.Country.List.First().NumericCode
                    },
                    AppUserData = new AppUserData
                    {
                        Upvotes = 10,
                        Downvotes = 5,
                        UserDescription = "Jeg er en veldig flink spiller!",
                        Followers = new[] { 2, 3, 4, 6, 7 },
                        Following = new[] { 2, 3, 4, 6, 7 },
                        UserFavoriteGames = new[] { 2, 3, 4, 6, 7 }
                    }
                }
            };

            var testUser = new AppUser
            {
                UserName = "membertest",
                Email = "member@test.com",
                AppUserProfile = new AppUserProfile
                {
                    Birthday = new DateTime(1998, 7, 30),
                    Gender = "Male",
                    CountryIso = new CountryIso
                    {
                        Name = ISO3166.Country.List.First().Name,
                        TwoLetterCode = ISO3166.Country.List.First().TwoLetterCode,
                        ThreeLetterCode = ISO3166.Country.List.First().ThreeLetterCode,
                        NumericCode = ISO3166.Country.List.First().NumericCode
                    },
                    AppUserData = new AppUserData
                    {
                        Upvotes = 10,
                        Downvotes = 5,
                        UserDescription = "Jeg er en veldig flink spiller!",
                        Followers = new[] { 2, 3, 4, 6, 7 },
                        Following = new[] { 2, 3, 4, 6, 7 },
                        UserFavoriteGames = new[] { 2, 3, 4, 6, 7 }
                    }
                }
            };

            await userManager.CreateAsync(admin, "Pa$$w0rd");
            await userManager.AddToRolesAsync(admin, new[] { "Admin", "Premium" });

            await userManager.CreateAsync(testUser, "Playfu123!");
            await userManager.AddToRolesAsync(testUser, new[] { Role.Member.MakeString() });
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

        /// <summary>
        /// Will seed all the countries to the database
        /// </summary>
        /// <param name="countryRepository"></param>
        /// <returns></returns>
        public static async Task SeedCountryIso(ICountryRepository countryRepository)
        {
            if (await countryRepository.GetCountryIsoByIdAsync(1) != null) return;

            foreach (Country country in ISO3166.Country.List)
            {
                var countryIso = new CountryIso
                {
                    Name = country.Name,
                    TwoLetterCode = country.TwoLetterCode,
                    ThreeLetterCode = country.ThreeLetterCode,
                    NumericCode = country.NumericCode
                };
                countryRepository.AddCountryIso(countryIso);
            }
            await countryRepository.SaveAllAsync();
            Console.WriteLine($"Finished seeding Country Data");
        }
    }
}