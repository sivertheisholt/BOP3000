using API.Data;
using API.Entities.Roles;
using API.Entities.Users;
using API.Helpers;
using API.Interfaces;
using API.Interfaces.IClients;
using API.Interfaces.IRepositories;
using API.Interfaces.IServices;
using API.SignalR;
using AutoMapper;
using Discord;
using Discord.WebSocket;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API
{
    public class Program
    {
        public static Task Main(string[] args) => new Program().MainAsync(args);

        public async Task MainAsync(string[] args)
        {
            await InitHost(args);
        }

        private static async Task InitHost(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            try
            {
                var contex = services.GetRequiredService<DataContext>();
                var userManager = services.GetRequiredService<UserManager<AppUser>>();
                var roleManager = services.GetRequiredService<RoleManager<AppRole>>();

                var steamStoreClient = services.GetRequiredService<ISteamStoreClient>();
                var steamAppsClient = services.GetRequiredService<ISteamAppsClient>();

                var meilisearchService = services.GetRequiredService<IMeilisearchService>();

                var lobbyHub = services.GetRequiredService<LobbyHub>();
                var lobbyChatHub = services.GetRequiredService<LobbyChatHub>();

                var mapper = services.GetRequiredService<IMapper>();

                var unitOfWork = services.GetRequiredService<IUnitOfWork>();

                await contex.Database.MigrateAsync();
                await Seed.SeedCountryIso(unitOfWork);
                await Seed.SeedUsers(userManager, roleManager, meilisearchService, mapper, unitOfWork);
                await Seed.SeedCustomSteamApps(steamStoreClient, steamAppsClient, unitOfWork);
                await Seed.SeedLobbies(lobbyHub, unitOfWork);
                await Seed.SeedLobbyHub(mapper, unitOfWork, lobbyHub, lobbyChatHub);
                await Seed.SeedActivities(unitOfWork);
                await Seed.SeedMeilisearch(meilisearchService, unitOfWork, mapper);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                var logger = services.GetRequiredService<ILogger>();
                logger.LogError(ex, "An error occured during migrations");
            }
            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
