using API.Entities.Activities;
using API.Entities.Applications;
using API.Entities.Countries;
using API.Entities.Lobbies;
using API.Entities.Roles;
using API.Entities.SteamApps;
using API.Entities.Users;
using API.Enums;
using API.Interfaces.IClients;
using API.Interfaces.IRepositories;
using API.Interfaces.IServices;
using API.SignalR;
using AutoMapper;
using ISO3166;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

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
        public static async Task SeedUsers(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, ICountryRepository countryRepository, IMeilisearchService meilisearchService, IUserRepository userRepository, IMapper mapper)
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

            using (StreamReader r = new StreamReader("Data/SeedData/Users.json"))
            {
                string json = r.ReadToEnd();
                List<AppUser> users = JsonConvert.DeserializeObject<List<AppUser>>(json);
                Random rnd = new Random();
                foreach (var user in users)
                {
                    user.AppUserProfile.CountryIso = await countryRepository.GetCountryIsoByIdAsync(rnd.Next(1, 50));

                    if (user.UserName == "Playfu1")
                    {
                        await userManager.CreateAsync(user, "Pa$$w0rd");
                        await userManager.AddToRolesAsync(user, new[] { Role.Member.MakeString(), Role.Admin.MakeString(), Role.Premium.MakeString() });
                    }
                    else
                    {
                        await userManager.CreateAsync(user, "Playfu123!");
                        await userManager.AddToRolesAsync(user, new[] { Role.Member.MakeString() });
                    }
                }
            }

            //Seed to search
            var createTask = meilisearchService.CreateIndexAsync("members");

            var usersMeili = mapper.Map<List<AppUserMeili>>(await userRepository.GetUsersMeiliAsync());

            var cont = createTask.ContinueWith(task =>
            {
                var index = meilisearchService.GetIndex("members");

                var docsTask = meilisearchService.AddDocumentsAsync(usersMeili.ToArray(), index);

                var docs = docsTask.ContinueWith(docsTask =>
                {
                    Console.WriteLine("Meilisearch docs successfully uploaded");
                });
                docs.Wait();
            });

            cont.Wait();
        }

        /// <summary>
        /// Will seed 10 games to the database for development and add all the apps to the database. 
        /// </summary>
        /// <param name="steamAppRepository"></param>
        /// <param name="steamStoreClient"></param>
        /// <param name="steamAppsClient"></param>
        /// <returns></returns>
        public static async Task SeedSteamApps(ISteamAppRepository steamAppRepository, ISteamAppsRepository steamAppsRepository,
                                                    ISteamStoreClient steamStoreClient, ISteamAppsClient steamAppsClient,
                                                        IMeilisearchService meilisearchService)
        {
            var max = 10;
            var counter = 0;

            var apps = await steamAppsClient.GetAppsList();


            if (await steamAppRepository.GetAppInfoAsync(1) != null) return;

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
            var createTask = meilisearchService.CreateIndexAsync("apps");

            var cont = createTask.ContinueWith(task =>
            {
                var index = meilisearchService.GetIndex("apps");

                var docsTask = meilisearchService.AddDocumentsAsync(apps.Apps.ToArray(), index);

                var docs = docsTask.ContinueWith(docsTask =>
                {
                    Console.WriteLine("Meilisearch docs successfully uploaded");
                });
                docs.Wait();
            });

            cont.Wait();

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

        public static async Task SeedSteamAppsInfo(ISteamAppRepository steamAppRepository, ISteamAppsRepository steamAppsRepository,
                                                    ISteamStoreClient steamStoreClient, ISteamAppsClient steamAppsClient)
        {
            if (await steamAppRepository.GetAppInfoAsync(51) != null) return;
            var apps = new Int32[] { 730, 1599340, 1172470, 381210, 427520 };
            foreach (var app in apps)
            {
                var appResult = await steamStoreClient.GetAppInfo(app);
                if (!appResult.Success) return;
                steamAppRepository.AddApp(appResult);
            }
            await steamAppRepository.SaveAllAsync();
            Console.WriteLine($"Finished seeding custom steam apps Data");
        }

        public static async Task SeedLobbies(ILobbiesRepository lobbiesRepository, LobbyHub lobbyHub, ISteamAppRepository steamAppRepository)
        {
            if (await lobbiesRepository.GetLobbyAsync(1) != null) return;

            using (StreamReader r = new StreamReader("Data/SeedData/Lobbies.json"))
            {
                string json = r.ReadToEnd();
                List<Lobby> lobbies = JsonConvert.DeserializeObject<List<Lobby>>(json);
                foreach (var lobby in lobbies)
                {
                    lobby.GameName = (await steamAppRepository.GetAppInfoAsync(lobby.GameId)).Data.Name;
                    lobbiesRepository.AddLobby(lobby);
                }
            }

            using (StreamReader r = new StreamReader("Data/SeedData/FinishedLobbies.json"))
            {
                string json = r.ReadToEnd();
                List<Lobby> lobbies = JsonConvert.DeserializeObject<List<Lobby>>(json);
                foreach (var lobby in lobbies)
                {
                    lobby.GameName = (await steamAppRepository.GetAppInfoAsync(lobby.GameId)).Data.Name;
                    lobbiesRepository.AddLobby(lobby);
                }
            }

            await lobbiesRepository.SaveAllAsync();

            Console.WriteLine($"Finished seeding lobbies data");
        }

        public static async Task SeedLobbyHub(ILobbiesRepository lobbiesRepository, LobbyHub lobbyHub)
        {
            await lobbyHub.CreateLobbyTest(1, 1);
            await lobbyHub.CreateLobbyTest(2, 5);
            await lobbyHub.CreateLobbyTest(3, 6);
            await lobbyHub.CreateLobbyTest(4, 7);
            await lobbyHub.CreateLobbyTest(5, 8);
            await lobbyHub.CreateLobbyTest(6, 9);
            await lobbyHub.CreateLobbyTest(7, 10);
            Console.WriteLine($"Finished seeding lobby hub data");
        }

        public static async Task SeedActivities(IActivitiesRepository activitiesRepository, IActivityRepository activityRepository)
        {
            if (await activityRepository.GetActivity(1) != null) return;

            using (StreamReader r = new StreamReader("Data/SeedData/Activities.json"))
            {
                string json = r.ReadToEnd();
                List<Activity> activities = JsonConvert.DeserializeObject<List<Activity>>(json);
                foreach (var activity in activities)
                {
                    activityRepository.AddActivity(activity);
                }
            }
            using (StreamReader r = new StreamReader("Data/SeedData/ActivityLogs.json"))
            {
                string json = r.ReadToEnd();
                List<ActivityLog> activityLogs = JsonConvert.DeserializeObject<List<ActivityLog>>(json);
                foreach (var activityLog in activityLogs)
                {
                    activitiesRepository.AddActivityLog(activityLog);
                }
            }

            await activityRepository.SaveAllAsync();
            await activitiesRepository.SaveAllAsync();
        }
    }
}