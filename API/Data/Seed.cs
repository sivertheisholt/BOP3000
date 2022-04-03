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

            var admin = new AppUser
            {
                UserName = "adminTest",
                Email = "admin@test.com",
                AppUserProfile = new AppUserProfile
                {
                    Birthday = new DateTime(1998, 7, 30),
                    Gender = "Male",
                    Description = "Jeg er en veldig flink spiller!",
                    CountryIso = await countryRepository.GetCountryIsoByIdAsync(1),
                    AppUserData = new AppUserData
                    {
                        Upvotes = 10,
                        Downvotes = 5,
                        Followers = new[] { 2, 3, 4, 6, 7 },
                        Following = new[] { 2, 3, 4, 6, 7 },
                        UserFavoriteGames = new[] { 2, 3, 4, 6, 7 },
                        FinishedLobbies = new[] { 1, 3 }
                    },
                    UserConnections = new AppUserConnections
                    {
                        SteamConnected = false,
                        DiscordConnected = false,
                        Steam = new Steam
                        {
                        },
                        Discord = new Discord
                        {
                        }
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
                    Description = "Jeg er en veldig flink spiller!",
                    CountryIso = await countryRepository.GetCountryIsoByIdAsync(1),
                    AppUserData = new AppUserData
                    {
                        Upvotes = 10,
                        Downvotes = 5,
                        Followers = new[] { 2, 3, 4, 6, 7 },
                        Following = new[] { 2, 3, 4, 6, 7 },
                        UserFavoriteGames = new[] { 2, 3, 4, 6, 7 },
                        FinishedLobbies = new[] { 2 }
                    },
                    UserConnections = new AppUserConnections
                    {
                        SteamConnected = false,
                        DiscordConnected = false,
                        Steam = new Steam
                        {
                        },
                        Discord = new Discord
                        {
                        }
                    }
                }
            };
            var testUser2 = new AppUser
            {
                UserName = "membertest2",
                Email = "member2@test.com",
                AppUserProfile = new AppUserProfile
                {
                    Birthday = new DateTime(1998, 7, 30),
                    Gender = "Male",
                    Description = "Jeg er en veldig flink spiller 2!",
                    CountryIso = await countryRepository.GetCountryIsoByIdAsync(1),
                    AppUserData = new AppUserData
                    {
                        Upvotes = 10,
                        Downvotes = 5,
                        Followers = new[] { 2, 3, 4, 6, 7 },
                        Following = new[] { 2, 3, 4, 6, 7 },
                        UserFavoriteGames = new[] { 2, 3, 4, 6, 7 }
                    },
                    UserConnections = new AppUserConnections
                    {
                        SteamConnected = false,
                        DiscordConnected = false,
                        Steam = new Steam
                        {
                        },
                        Discord = new Discord
                        {
                        }
                    }
                }
            };

            var testUser3 = new AppUser
            {
                UserName = "membertest3",
                Email = "member3@test.com",
                AppUserProfile = new AppUserProfile
                {
                    Birthday = new DateTime(1998, 7, 30),
                    Gender = "Male",
                    Description = "Jeg er en veldig flink spiller 3!",
                    CountryIso = await countryRepository.GetCountryIsoByIdAsync(1),
                    AppUserData = new AppUserData
                    {
                        Upvotes = 10,
                        Downvotes = 5,
                        Followers = new[] { 2, 3, 4, 6, 7 },
                        Following = new[] { 2, 3, 4, 6, 7 },
                        UserFavoriteGames = new[] { 2, 3, 4, 6, 7 }
                    },
                    UserConnections = new AppUserConnections
                    {
                        SteamConnected = false,
                        DiscordConnected = false,
                        Steam = new Steam
                        {
                        },
                        Discord = new Discord
                        {
                        }
                    }
                }
            };

            var testUser4 = new AppUser
            {
                UserName = "membertest4",
                Email = "member4@test.com",
                AppUserProfile = new AppUserProfile
                {
                    Birthday = new DateTime(1998, 7, 30),
                    Gender = "Male",
                    Description = "Jeg er en veldig flink spiller 4!",
                    CountryIso = await countryRepository.GetCountryIsoByIdAsync(1),
                    AppUserData = new AppUserData
                    {
                        Upvotes = 10,
                        Downvotes = 5,
                        Followers = new[] { 2, 3, 4, 6, 7 },
                        Following = new[] { 2, 3, 4, 6, 7 },
                        UserFavoriteGames = new[] { 2, 3, 4, 6, 7 }
                    },
                    UserConnections = new AppUserConnections
                    {
                        SteamConnected = false,
                        DiscordConnected = false,
                        Steam = new Steam
                        {
                        },
                        Discord = new Discord
                        {
                        }
                    }
                }
            };
            var testUser5 = new AppUser
            {
                UserName = "membertest5",
                Email = "member5@test.com",
                AppUserProfile = new AppUserProfile
                {
                    Birthday = new DateTime(1998, 7, 30),
                    Gender = "Male",
                    Description = "Jeg er en veldig flink spiller 5!",
                    CountryIso = await countryRepository.GetCountryIsoByIdAsync(1),
                    AppUserData = new AppUserData
                    {
                        Upvotes = 10,
                        Downvotes = 5,
                        Followers = new[] { 2, 3, 4, 6, 7 },
                        Following = new[] { 2, 3, 4, 6, 7 },
                        UserFavoriteGames = new[] { 2, 3, 4, 6, 7 }
                    },
                    UserConnections = new AppUserConnections
                    {
                        SteamConnected = false,
                        DiscordConnected = false,
                        Steam = new Steam
                        {
                        },
                        Discord = new Discord
                        {
                        }
                    }
                }
            };

            var testUser6 = new AppUser
            {
                UserName = "membertest6",
                Email = "member6@test.com",
                AppUserProfile = new AppUserProfile
                {
                    Birthday = new DateTime(1998, 7, 30),
                    Gender = "Male",
                    Description = "Jeg er en veldig flink spiller 6!",
                    CountryIso = await countryRepository.GetCountryIsoByIdAsync(1),
                    AppUserData = new AppUserData
                    {
                        Upvotes = 10,
                        Downvotes = 5,
                        Followers = new[] { 2, 3, 4, 6, 7 },
                        Following = new[] { 2, 3, 4, 6, 7 },
                        UserFavoriteGames = new[] { 2, 3, 4, 6, 7 }
                    },
                    UserConnections = new AppUserConnections
                    {
                        SteamConnected = false,
                        DiscordConnected = false,
                        Steam = new Steam
                        {
                        },
                        Discord = new Discord
                        {
                        }
                    }
                }
            };

            await userManager.CreateAsync(admin, "Pa$$w0rd");
            await userManager.AddToRolesAsync(admin, new[] { "Member", "Admin", "Premium" });

            await userManager.CreateAsync(testUser, "Playfu123!");
            await userManager.AddToRolesAsync(testUser, new[] { Role.Member.MakeString() });

            await userManager.CreateAsync(testUser2, "Playfu123!");
            await userManager.AddToRolesAsync(testUser2, new[] { Role.Member.MakeString() });

            await userManager.CreateAsync(testUser3, "Playfu123!");
            await userManager.AddToRolesAsync(testUser3, new[] { Role.Member.MakeString() });

            await userManager.CreateAsync(testUser4, "Playfu123!");
            await userManager.AddToRolesAsync(testUser4, new[] { Role.Member.MakeString() });

            await userManager.CreateAsync(testUser5, "Playfu123!");
            await userManager.AddToRolesAsync(testUser5, new[] { Role.Member.MakeString() });

            await userManager.CreateAsync(testUser6, "Playfu123!");
            await userManager.AddToRolesAsync(testUser6, new[] { Role.Member.MakeString() });

            //Seed to search
            var createTask = meilisearchService.CreateIndexAsync("members");

            var users = mapper.Map<List<AppUserMeili>>(await userRepository.GetUsersMeiliAsync());

            var cont = createTask.ContinueWith(task =>
            {
                var index = meilisearchService.GetIndex("members");

                var docsTask = meilisearchService.AddDocumentsAsync(users.ToArray(), index);

                var docs = docsTask.ContinueWith(docsTask =>
                {
                    Console.WriteLine("Meilisearch docs successfully uploaded");
                });
                docs.Wait();
            });

            cont.Wait();

            Console.WriteLine($"Finished seeding user Data");
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

            var lobbies = new Lobby[] {
                new Lobby {MaxUsers = 5,
                            AdminUid = 1,
                            Title = "Whats up gamers",
                            LobbyDescription = "Hello there",
                            GameId=51,
                            GameName = (await steamAppRepository.GetAppInfoAsync(51)).Data.Name,
                            GameType="Competetive",
                            LobbyRequirement = new Requirement {
                                Gender = "Male"
                            },
                            StartDate = DateTime.Now
                            },

                new Lobby {MaxUsers = 5,
                            AdminUid = 3,
                            Title = "Hey guys lets play",
                            LobbyDescription = "Sup",
                            GameId=51,
                            GameName = (await steamAppRepository.GetAppInfoAsync(51)).Data.Name,
                            GameType="Casual",
                            LobbyRequirement = new Requirement {
                                Gender = "Male"
                            },
                            StartDate = DateTime.Now},
                new Lobby {MaxUsers = 5,
                            AdminUid = 4,
                            Title = "Whats up noobs",
                            LobbyDescription = "Hmmm",
                            GameId=52,
                            GameName = (await steamAppRepository.GetAppInfoAsync(52)).Data.Name,
                            GameType="Competetive",
                            LobbyRequirement = new Requirement {
                                Gender = "Female"
                            },
                            StartDate = DateTime.Now},
                new Lobby {MaxUsers = 5,
                            AdminUid = 5,
                            Title = "Halla",
                            LobbyDescription = "I dont know",
                            GameId=53,
                            GameName = (await steamAppRepository.GetAppInfoAsync(53)).Data.Name,
                            GameType="Casual",
                            LobbyRequirement = new Requirement {
                                Gender = "Male"
                            },
                            StartDate = DateTime.Now},
                new Lobby
                {
                    MaxUsers = 5,
                    AdminUid = 6,
                    Title = "Play smth?",
                    LobbyDescription = "Sheeeeesh",
                    GameId=54,
                    GameName = (await steamAppRepository.GetAppInfoAsync(54)).Data.Name,
                    GameType="Casual",
                    LobbyRequirement = new Requirement {
                        Gender = "Female"
                    },
                    StartDate = DateTime.Now
                    },

                 new Lobby
                 {
                    MaxUsers = 5,
                    AdminUid = 2,
                    Title = "Play smth?",
                    LobbyDescription = "Hah",
                    GameId=54,
                    GameName = (await steamAppRepository.GetAppInfoAsync(54)).Data.Name,
                    GameType="Casual",
                    LobbyRequirement = new Requirement {
                        Gender = "Female"
                    },
                    Log = new Log{

                    },
                    FinishedDate = DateTime.Now,
                    StartDate = DateTime.Now,
                    Users = new List<int>(){1, 2, 3},
                    Finished = true
                },
                new Lobby
                {
                    MaxUsers = 5,
                    AdminUid = 1,
                    Title = "The fuck are you looking at",
                    LobbyDescription = "Sheeeeesh",
                    GameId=54,
                    GameName = (await steamAppRepository.GetAppInfoAsync(54)).Data.Name,
                    GameType="Casual",
                    LobbyRequirement = new Requirement {
                        Gender = "Male"
                    },
                    Log = new Log{

                    },
                    FinishedDate = DateTime.Now,
                    StartDate = DateTime.Now,
                    Users = new List<int>(){1, 3, 5},
                    Finished = true
                },
                new Lobby
                {
                    MaxUsers = 5,
                    AdminUid = 6,
                    Title = "Lol noob",
                    LobbyDescription = "Lmao",
                    GameId=53,
                    GameName = (await steamAppRepository.GetAppInfoAsync(53)).Data.Name,
                    GameType="Competetive",
                    LobbyRequirement = new Requirement {
                        Gender = "Female"
                    },
                    Log = new Log{

                    },
                    FinishedDate = DateTime.Now,
                    StartDate = DateTime.Now,
                    Users = new List<int>(){2, 4, 6},
                    Finished = true
                }
            };

            foreach (var item in lobbies)
            {
                lobbiesRepository.AddLobby(item);
            }
            await lobbiesRepository.SaveAllAsync();
            Console.WriteLine($"Finished seeding lobbies data");
        }
        public static async Task SeedLobbyHub(ILobbiesRepository lobbiesRepository, LobbyHub lobbyHub)
        {
            await lobbyHub.CreateLobbyTest(1, 1);
            await lobbyHub.CreateLobbyTest(2, 7);
            for (int i = 3; i < 6; i++)
            {
                await lobbyHub.CreateLobbyTest(i, i);
            }
            Console.WriteLine($"Finished seeding lobby hub data");
        }

        public static async Task SeedActivities(IActivitiesRepository activitiesRepository, IActivityRepository activityRepository)
        {
            if (await activityRepository.GetActivity(1) != null) return;

            var activityTypes = new Activity[] {
                new Activity {
                    Type = "Lobby",
                    Identifier = "lobby-finished",
                    Text = "finished lobby"
                },
                new Activity {
                    Type = "Lobby",
                    Identifier = "lobby-created",
                    Text = "created lobby"
                },
                new Activity {
                    Type = "Lobby",
                    Identifier = "lobby-joined",
                    Text = "joined lobby"
                },
                new Activity {
                    Type = "Member",
                    Identifier = "member-followed",
                    Text = "followed"
                }
            };

            foreach (var activity in activityTypes)
            {
                await activityRepository.AddActivity(activity);
            }
            await activityRepository.SaveAllAsync();

            var activityLogs = new ActivityLog[] {
                new ActivityLog {
                    Date = new DateTime(2022, 3, 27),
                    AppUserId = 3,
                    ActivityId = 1
                },
                new ActivityLog {
                    Date = new DateTime(2022, 3, 27),
                    AppUserId = 2,
                    ActivityId = 1
                },
                new ActivityLog {
                    Date = new DateTime(2022, 3, 27),
                    AppUserId = 2,
                    ActivityId = 2
                },
                new ActivityLog {
                    Date = new DateTime(2022, 3, 28),
                    AppUserId = 6,
                    ActivityId = 3
                },
                new ActivityLog {
                    Date = new DateTime(2022, 3, 28),
                    AppUserId = 2,
                    ActivityId = 4
                },
                new ActivityLog {
                    Date = new DateTime(2022, 3, 29),
                    AppUserId = 2,
                    ActivityId = 2
                },
                new ActivityLog {
                    Date = new DateTime(2022, 3, 29),
                    AppUserId = 2,
                    ActivityId = 3
                },
                new ActivityLog {
                    Date = new DateTime(2022, 3, 30),
                    AppUserId = 2,
                    ActivityId = 1
                },
                new ActivityLog {
                    Date = new DateTime(2022, 3, 30),
                    AppUserId = 3,
                    ActivityId = 3
                },
                new ActivityLog {
                    Date = new DateTime(2022, 3, 31),
                    AppUserId = 3,
                    ActivityId = 2
                }
            };

            foreach (var log in activityLogs)
            {
                await activitiesRepository.AddActivityLog(log);
            }
            await activitiesRepository.SaveAllAsync();
        }
    }
}