using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities.Activities;
using API.Entities.Countries;
using API.Entities.Lobbies;
using API.Entities.Roles;
using API.Entities.SteamApps;
using API.Entities.Users;
using API.Enums;
using API.Interfaces.IClients;
using API.Interfaces.IRepositories;
using API.Interfaces.IServices;
using AutoMapper;
using ISO3166;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace API.Controllers
{
    public class SeedController : BaseApiController
    {
        private readonly IUserRepository _userRespository;
        private readonly ILobbiesRepository _lobbiesRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly ISteamAppRepository _steamAppRepository;
        private readonly IMeilisearchService _meilisearchService;
        private readonly IActivitiesRepository _activitiesRepository;
        private readonly IActivityRepository _activityRepository;
        private readonly ISteamAppsRepository _steamAppsRepository;
        private readonly ISteamStoreClient _steamStoreClient;
        private readonly ISteamAppsClient _steamAppsClient;
        public SeedController(IMapper mapper, IUserRepository userRespository, ILobbiesRepository lobbiesRepository, ICountryRepository countryRepository, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, ISteamAppRepository steamAppRepository, IMeilisearchService meilisearchService, IActivitiesRepository activitiesRepository, IActivityRepository activityRepository, ISteamAppsRepository steamAppsRepository, ISteamStoreClient steamStoreClient, ISteamAppsClient steamAppsClient) : base(mapper)
        {
            _steamAppsClient = steamAppsClient;
            _steamStoreClient = steamStoreClient;
            _steamAppsRepository = steamAppsRepository;
            _activityRepository = activityRepository;
            _activitiesRepository = activitiesRepository;
            _meilisearchService = meilisearchService;
            _steamAppRepository = steamAppRepository;
            _roleManager = roleManager;
            _userManager = userManager;
            _countryRepository = countryRepository;
            _lobbiesRepository = lobbiesRepository;
            _userRespository = userRespository;
        }

        [HttpPatch("seed_users")]
        public async Task<ActionResult> SeedUsers()
        {
            var currentUsers = await _userRespository.GetAllUsers();
            foreach (var user in currentUsers)
            {
                _userRespository.Delete(user);
            }

            await _userRespository.resetId("AspNetUsers");

            await _userRespository.SaveAllAsync();

            using (StreamReader r = new StreamReader("Data/SeedData/Users.json"))
            {
                string json = r.ReadToEnd();
                List<AppUser> users = JsonConvert.DeserializeObject<List<AppUser>>(json);
                Random rnd = new Random();
                foreach (var user in users)
                {
                    user.AppUserProfile.CountryIso = await _countryRepository.GetCountryIsoByIdAsync(rnd.Next(1, 50));

                    if (user.UserName == "Playfu1")
                    {
                        await _userManager.CreateAsync(user, "Pa$$w0rd");
                        await _userManager.AddToRolesAsync(user, new[] { Role.Member.MakeString(), Role.Admin.MakeString(), Role.Premium.MakeString() });
                    }
                    else
                    {
                        await _userManager.CreateAsync(user, "Playfu123!");
                        await _userManager.AddToRolesAsync(user, new[] { Role.Member.MakeString() });
                    }
                }
            }

            await _userRespository.SaveAllAsync();

            //Seed to search
            var createTask = _meilisearchService.CreateIndexAsync("members");

            var usersMeili = Mapper.Map<List<AppUserMeili>>(await _userRespository.GetUsersMeiliAsync());

            var cont = createTask.ContinueWith(task =>
            {
                var index = _meilisearchService.GetIndex("members");

                var docsTask = _meilisearchService.AddDocumentsAsync(usersMeili.ToArray(), index);

                var docs = docsTask.ContinueWith(docsTask =>
                {
                    Console.WriteLine("Meilisearch docs successfully uploaded");
                });
                docs.Wait();
            });

            cont.Wait();

            return NoContent();
        }

        [HttpPatch("seed_lobbies")]
        public async Task<ActionResult> SeedLobbies()
        {
            var currentLobbies = await _lobbiesRepository.GetLobbiesAsync();
            foreach (var lobby in currentLobbies)
            {
                _lobbiesRepository.Delete(lobby);
            }

            await _lobbiesRepository.resetId("Lobby");

            await _lobbiesRepository.SaveAllAsync();

            using (StreamReader r = new StreamReader("Data/SeedData/Lobbies.json"))
            {
                string json = r.ReadToEnd();
                List<Lobby> lobbies = JsonConvert.DeserializeObject<List<Lobby>>(json);
                foreach (var lobby in lobbies)
                {
                    lobby.GameName = (await _steamAppRepository.GetAppInfoAsync(lobby.GameId)).Data.Name;
                    _lobbiesRepository.AddLobby(lobby);
                }
            }

            using (StreamReader r = new StreamReader("Data/SeedData/FinishedLobbies.json"))
            {
                string json = r.ReadToEnd();
                List<Lobby> lobbies = JsonConvert.DeserializeObject<List<Lobby>>(json);
                foreach (var lobby in lobbies)
                {
                    lobby.GameName = (await _steamAppRepository.GetAppInfoAsync(lobby.GameId)).Data.Name;
                    _lobbiesRepository.AddLobby(lobby);
                }
            }

            await _lobbiesRepository.SaveAllAsync();

            return NoContent();
        }

        [HttpPatch("seed_activities")]
        public async Task<ActionResult> SeedActivities()
        {
            var CurrentActivities = await _activityRepository.GetActivities();
            foreach (var activity in CurrentActivities)
            {
                _activityRepository.Delete(activity);
            }

            var currentActivityLogs = await _activitiesRepository.GetActivities();
            foreach (var activityLog in currentActivityLogs)
            {
                _activitiesRepository.Delete(activityLog);
            }

            await _activityRepository.SaveAllAsync();
            await _activitiesRepository.SaveAllAsync();

            await _activityRepository.resetId("Activity");
            await _activitiesRepository.resetId("ActivityLog");

            using (StreamReader r = new StreamReader("Data/SeedData/Activities.json"))
            {
                string json = r.ReadToEnd();
                List<Activity> activities = JsonConvert.DeserializeObject<List<Activity>>(json);
                foreach (var activity in activities)
                {
                    _activityRepository.AddActivity(activity);
                }
            }
            using (StreamReader r = new StreamReader("Data/SeedData/ActivityLogs.json"))
            {
                string json = r.ReadToEnd();
                List<ActivityLog> activityLogs = JsonConvert.DeserializeObject<List<ActivityLog>>(json);
                foreach (var activityLog in activityLogs)
                {
                    _activitiesRepository.AddActivityLog(activityLog);
                }
            }

            await _activityRepository.SaveAllAsync();
            await _activitiesRepository.SaveAllAsync();

            return NoContent();
        }
        [HttpPatch("seed_apps")]
        public async Task<ActionResult> SeedApps()
        {
            var max = 10;
            var counter = 0;

            var currentApps = await _steamAppRepository.GetAllApps();

            foreach (var app in currentApps)
            {
                _steamAppRepository.Delete(app);
            }

            await _steamAppRepository.SaveAllAsync();
            await _steamAppRepository.resetId("AppInfo");

            var currentAppsList = await _steamAppsRepository.GetAllAppsList();

            foreach (var appsList in currentAppsList)
            {
                _steamAppsRepository.Delete(appsList);
            }

            await _steamAppsRepository.SaveAllAsync();
            await _steamAppsRepository.resetId("AppList");

            var apps = await _steamAppsClient.GetAppsList();

            _steamAppsRepository.AddAppsList(apps);

            foreach (AppListInfo app in apps.Apps)
            {
                if (counter >= max) break;

                var gameResult = await _steamStoreClient.GetAppInfo(app.AppId);
                if (!gameResult.Success) continue;

                _steamAppRepository.AddApp(gameResult);

                counter++;
            }

            var appsCustom = new Int32[] { 730, 1599340, 1172470, 381210, 427520 };

            foreach (var app in appsCustom)
            {
                var appResult = await _steamStoreClient.GetAppInfo(app);
                _steamAppRepository.AddApp(appResult);
            }

            await _steamAppRepository.SaveAllAsync();
            await _steamAppsRepository.SaveAllAsync();

            //Seed to search
            var createTask = _meilisearchService.CreateIndexAsync("apps");

            var cont = createTask.ContinueWith(task =>
            {
                var index = _meilisearchService.GetIndex("apps");

                var docsTask = _meilisearchService.AddDocumentsAsync(apps.Apps.ToArray(), index);

                var docs = docsTask.ContinueWith(docsTask =>
                {
                    Console.WriteLine("Meilisearch docs successfully uploaded");
                });
                docs.Wait();
            });

            cont.Wait();

            Console.WriteLine($"Finished seeding Steam data");

            return NoContent();
        }
    }
}