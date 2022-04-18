using API.Entities.Activities;
using API.Entities.Applications;
using API.Entities.Countries;
using API.Entities.Lobbies;
using API.Entities.Roles;
using API.Entities.SteamApps;
using API.Entities.Users;
using API.Enums;
using API.Helpers.PaginationsParams;
using API.Interfaces;
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
        public static async Task SeedUsers(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, IMeilisearchService meilisearchService, IMapper mapper, IUnitOfWork unitOfWork)
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
                    user.AppUserProfile.CountryIso = await unitOfWork.countryRepository.GetCountryIsoByIdAsync(rnd.Next(1, 50));

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

            var usersMeili = mapper.Map<List<AppUserMeili>>(await unitOfWork.userRepository.GetUsersMeiliAsync());

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
        /// <param name="unitOfWork.steamAppRepository"></param>
        /// <param name="steamStoreClient"></param>
        /// <param name="steamAppsClient"></param>
        /// <returns></returns>
        public static async Task SeedSteamApps(ISteamStoreClient steamStoreClient, ISteamAppsClient steamAppsClient, IMeilisearchService meilisearchService, IUnitOfWork unitOfWork)
        {
            var max = 10;
            var counter = 0;

            var apps = await steamAppsClient.GetAppsList();


            if (await unitOfWork.steamAppRepository.GetAppInfoAsync(1) != null) return;

            unitOfWork.steamAppsRepository.AddAppsList(apps);

            foreach (AppListInfo app in apps.Apps)
            {
                if (counter >= max) break;

                var gameResult = await steamStoreClient.GetAppInfo(app.AppId);
                if (!gameResult.Success) continue;

                unitOfWork.steamAppRepository.AddApp(gameResult);

                counter++;
            }

            await unitOfWork.Complete();

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
        /// <param name="unitOfWork.countryRepository"></param>
        /// <returns></returns>
        public static async Task SeedCountryIso(IUnitOfWork unitOfWork)
        {
            if (await unitOfWork.countryRepository.GetCountryIsoByIdAsync(1) != null) return;

            foreach (Country country in ISO3166.Country.List)
            {
                var countryIso = new CountryIso
                {
                    Name = country.Name,
                    TwoLetterCode = country.TwoLetterCode,
                    ThreeLetterCode = country.ThreeLetterCode,
                    NumericCode = country.NumericCode
                };
                unitOfWork.countryRepository.AddCountryIso(countryIso);
            }
            await unitOfWork.Complete();
            Console.WriteLine($"Finished seeding Country Data");
        }

        public static async Task SeedCustomSteamApps(ISteamStoreClient steamStoreClient, ISteamAppsClient steamAppsClient, IUnitOfWork unitOfWork)
        {
            if (await unitOfWork.steamAppRepository.GetAppInfoAsync(1) != null) return;
            var apps = new Int32[] { 730, 1599340, 1172470, 381210, 427520 };
            foreach (var app in apps)
            {
                var appResult = await steamStoreClient.GetAppInfo(app);
                if (!appResult.Success) return;
                unitOfWork.steamAppRepository.AddApp(appResult);
            }
            await unitOfWork.Complete();
            Console.WriteLine($"Finished seeding custom steam apps Data");
        }

        public static async Task SeedLobbies(LobbyHub lobbyHub, IUnitOfWork unitOfWork)
        {
            if (await unitOfWork.lobbiesRepository.GetLobbyAsync(1) != null) return;

            using (StreamReader r = new StreamReader("Data/SeedData/Lobbies.json"))
            {
                string json = r.ReadToEnd();
                List<Lobby> lobbies = JsonConvert.DeserializeObject<List<Lobby>>(json);
                foreach (var lobby in lobbies)
                {
                    lobby.GameName = (await unitOfWork.steamAppRepository.GetAppInfoAsync(lobby.GameId)).Data.Name;
                    unitOfWork.lobbiesRepository.AddLobby(lobby);
                }
            }

            using (StreamReader r = new StreamReader("Data/SeedData/FinishedLobbies.json"))
            {
                string json = r.ReadToEnd();
                List<Lobby> lobbies = JsonConvert.DeserializeObject<List<Lobby>>(json);
                foreach (var lobby in lobbies)
                {
                    lobby.GameName = (await unitOfWork.steamAppRepository.GetAppInfoAsync(lobby.GameId)).Data.Name;
                    unitOfWork.lobbiesRepository.AddLobby(lobby);
                }
            }

            await unitOfWork.Complete();

            Console.WriteLine($"Finished seeding lobbies data");
        }

        public static async Task SeedLobbyHub(IUnitOfWork unitOfWork, LobbyHub lobbyHub)
        {
            var testLobbies = await unitOfWork.lobbiesRepository.GetActiveLobbies(new UniversalParams { });
            foreach (var lobby in testLobbies)
            {
                await lobbyHub.CreateLobbyTest(lobby, lobby.AdminUid);
            }
            Console.WriteLine($"Finished seeding lobby hub data");
        }

        public static async Task SeedActivities(IUnitOfWork unitOfWork)
        {
            if (await unitOfWork.activityRepository.GetActivity(1) != null) return;

            using (StreamReader r = new StreamReader("Data/SeedData/Activities.json"))
            {
                string json = r.ReadToEnd();
                List<Activity> activities = JsonConvert.DeserializeObject<List<Activity>>(json);
                foreach (var activity in activities)
                {
                    unitOfWork.activityRepository.AddActivity(activity);
                }
            }
            using (StreamReader r = new StreamReader("Data/SeedData/ActivityLogs.json"))
            {
                string json = r.ReadToEnd();
                List<ActivityLog> activityLogs = JsonConvert.DeserializeObject<List<ActivityLog>>(json);
                foreach (var activityLog in activityLogs)
                {
                    unitOfWork.activitiesRepository.AddActivityLog(activityLog);
                }
            }

            await unitOfWork.Complete();
        }

        public static async Task SeedMeilisearch(IMeilisearchService meilisearchService, IUnitOfWork unitOfWork, IMapper mapper)
        {
            var apps = await unitOfWork.steamAppsRepository.GetAppsList(1);
            if (apps == null) return;
            //Seed apps to search
            var createTaskApps = meilisearchService.CreateIndexAsync("apps");

            var contApps = createTaskApps.ContinueWith(task =>
            {
                var index = meilisearchService.GetIndex("apps");

                var docsTask = meilisearchService.AddDocumentsAsync(apps.Apps.ToArray(), index);

                var docs = docsTask.ContinueWith(docsTask =>
                {
                    Console.WriteLine("Meilisearch apps docs successfully uploaded");
                });
                docs.Wait();
            });

            contApps.Wait();

            //Seed members to search

            var createTaskMembers = meilisearchService.CreateIndexAsync("members");

            var usersMeili = mapper.Map<List<AppUserMeili>>(await unitOfWork.userRepository.GetUsersMeiliAsync());

            var contMembers = createTaskMembers.ContinueWith(task =>
            {
                var index = meilisearchService.GetIndex("members");

                var docsTask = meilisearchService.AddDocumentsAsync(usersMeili.ToArray(), index);

                var docs = docsTask.ContinueWith(docsTask =>
                {
                    Console.WriteLine("Meilisearch member docs successfully uploaded");
                });
                docs.Wait();
            });

            contMembers.Wait();
        }
    }
}