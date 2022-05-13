using API.Entities.Activities;
using API.Entities.Lobbies;
using API.Entities.Roles;
using API.Entities.SteamApps;
using API.Entities.Users;
using API.Enums;
using API.Helpers.PaginationsParams;
using API.Interfaces;
using API.Interfaces.IClients;
using API.Interfaces.IServices;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace API.Controllers
{
    public class SeedController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMeilisearchService _meilisearchService;
        private readonly ISteamStoreClient _steamStoreClient;
        private readonly ISteamAppsClient _steamAppsClient;
        private readonly IUnitOfWork _unitOfWork;
        public SeedController(IMapper mapper, UserManager<AppUser> userManager, IMeilisearchService meilisearchService, ISteamStoreClient steamStoreClient, ISteamAppsClient steamAppsClient, IUnitOfWork unitOfWork) : base(mapper)
        {
            _unitOfWork = unitOfWork;
            _steamAppsClient = steamAppsClient;
            _steamStoreClient = steamStoreClient;
            _meilisearchService = meilisearchService;
            _userManager = userManager;
        }

        [HttpPatch("seed_users")]
        public async Task<ActionResult> SeedUsers([FromQuery] MemberParams memberParams)
        {
            var currentUsers = await _unitOfWork.userRepository.GetAllUsersNoPaging();
            foreach (var user in currentUsers)
            {
                _unitOfWork.userRepository.Delete(user);
            }

            await _unitOfWork.userRepository.resetId("AspNetUsers");

            await _unitOfWork.Complete();

            using (StreamReader r = new StreamReader("Data/SeedData/Users.json"))
            {
                string json = r.ReadToEnd();
                List<AppUser> users = JsonConvert.DeserializeObject<List<AppUser>>(json);
                Random rnd = new Random();
                foreach (var user in users)
                {
                    user.AppUserProfile.CountryIso = await _unitOfWork.countryRepository.GetCountryIsoByIdAsync(rnd.Next(1, 50));

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

            await _unitOfWork.Complete();

            //Seed to search
            var createTask = _meilisearchService.CreateIndexAsync("members");

            var usersMeili = Mapper.Map<List<AppUserMeili>>(await _unitOfWork.userRepository.GetUsersMeiliAsync());

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
            var currentLobbies = await _unitOfWork.lobbiesRepository.GetAllLobbiesNoPaging();
            foreach (var lobby in currentLobbies)
            {
                _unitOfWork.lobbiesRepository.Delete(lobby);
            }

            await _unitOfWork.lobbiesRepository.resetId("Lobby");

            await _unitOfWork.Complete();

            using (StreamReader r = new StreamReader("Data/SeedData/Lobbies.json"))
            {
                string json = r.ReadToEnd();
                List<Lobby> lobbies = JsonConvert.DeserializeObject<List<Lobby>>(json);
                foreach (var lobby in lobbies)
                {
                    lobby.GameName = (await _unitOfWork.steamAppRepository.GetAppInfoAsync(lobby.GameId)).Data.Name;
                    _unitOfWork.lobbiesRepository.AddLobby(lobby);
                }
            }

            using (StreamReader r = new StreamReader("Data/SeedData/FinishedLobbies.json"))
            {
                string json = r.ReadToEnd();
                List<Lobby> lobbies = JsonConvert.DeserializeObject<List<Lobby>>(json);
                foreach (var lobby in lobbies)
                {
                    lobby.GameName = (await _unitOfWork.steamAppRepository.GetAppInfoAsync(lobby.GameId)).Data.Name;
                    _unitOfWork.lobbiesRepository.AddLobby(lobby);
                }
            }

            await _unitOfWork.Complete();

            return NoContent();
        }

        [HttpPatch("seed_activities")]
        public async Task<ActionResult> SeedActivities()
        {
            var CurrentActivities = await _unitOfWork.activityRepository.GetActivities();
            foreach (var activity in CurrentActivities)
            {
                _unitOfWork.activityRepository.Delete(activity);
            }

            var currentActivityLogs = await _unitOfWork.activitiesRepository.GetActivities();
            foreach (var activityLog in currentActivityLogs)
            {
                _unitOfWork.activitiesRepository.Delete(activityLog);
            }

            await _unitOfWork.Complete();
            await _unitOfWork.Complete();

            await _unitOfWork.activityRepository.resetId("Activity");
            await _unitOfWork.activitiesRepository.resetId("ActivityLog");

            using (StreamReader r = new StreamReader("Data/SeedData/Activities.json"))
            {
                string json = r.ReadToEnd();
                List<Activity> activities = JsonConvert.DeserializeObject<List<Activity>>(json);
                foreach (var activity in activities)
                {
                    _unitOfWork.activityRepository.AddActivity(activity);
                }
            }
            using (StreamReader r = new StreamReader("Data/SeedData/ActivityLogs.json"))
            {
                string json = r.ReadToEnd();
                List<ActivityLog> activityLogs = JsonConvert.DeserializeObject<List<ActivityLog>>(json);
                foreach (var activityLog in activityLogs)
                {
                    _unitOfWork.activitiesRepository.AddActivityLog(activityLog);
                }
            }

            await _unitOfWork.Complete();
            await _unitOfWork.Complete();

            return NoContent();
        }
        [HttpPatch("seed_apps")]
        public async Task<ActionResult> SeedApps()
        {
            var max = 10;
            var counter = 0;

            var currentApps = await _unitOfWork.steamAppRepository.GetAllApps(new UniversalParams { });

            foreach (var app in currentApps)
            {
                _unitOfWork.steamAppRepository.Delete(app);
            }

            await _unitOfWork.Complete();
            await _unitOfWork.steamAppRepository.resetId("AppInfo");

            var currentAppsList = await _unitOfWork.steamAppsRepository.GetAllAppsList();

            foreach (var appsList in currentAppsList)
            {
                _unitOfWork.steamAppsRepository.Delete(appsList);
            }

            await _unitOfWork.Complete();
            await _unitOfWork.steamAppsRepository.resetId("AppList");

            var apps = await _steamAppsClient.GetAppsList();

            _unitOfWork.steamAppsRepository.AddAppsList(apps);

            foreach (AppListInfo app in apps.Apps)
            {
                if (counter >= max) break;

                var gameResult = await _steamStoreClient.GetAppInfo(app.AppId);
                if (!gameResult.Success) continue;

                _unitOfWork.steamAppRepository.AddApp(gameResult);

                counter++;
            }

            var appsCustom = new Int32[] { 730, 1599340, 1172470, 381210, 427520 };

            foreach (var app in appsCustom)
            {
                var appResult = await _steamStoreClient.GetAppInfo(app);
                _unitOfWork.steamAppRepository.AddApp(appResult);
            }

            await _unitOfWork.Complete();

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