using API.Data;
using API.Entities.Roles;
using API.Entities.Users;
using API.Interfaces.IClients;
using API.Interfaces.IRepositories;
using API.Interfaces.IServices;
using API.SignalR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            try
            {
                var contex = services.GetRequiredService<DataContext>();
                var userManager = services.GetRequiredService<UserManager<AppUser>>();
                var roleManager = services.GetRequiredService<RoleManager<AppRole>>();

                var steamAppRepository = services.GetRequiredService<ISteamAppRepository>();
                var steamAppsrepository = services.GetRequiredService<ISteamAppsRepository>();

                var steamStoreClient = services.GetRequiredService<ISteamStoreClient>();
                var steamAppsClient = services.GetRequiredService<ISteamAppsClient>();

                var meilisearchService = services.GetRequiredService<IMeilisearchService>();

                var countryRepository = services.GetRequiredService<ICountryRepository>();
                var lobbiesRepository = services.GetRequiredService<ILobbiesRepository>();
                var finishedLobbiesRepository = services.GetRequiredService<IFinishedLobbyRepository>();

                var lobbyHub = services.GetRequiredService<LobbyHub>();

                await contex.Database.MigrateAsync();
                await Seed.SeedCountryIso(countryRepository);
                await Seed.SeedUsers(userManager, roleManager, countryRepository);
                await Seed.SeedSteamApps(steamAppRepository, steamAppsrepository, steamStoreClient, steamAppsClient, meilisearchService);
                await Seed.SeedSteamAppsInfo(steamAppRepository, steamAppsrepository, steamStoreClient, steamAppsClient);
                await Seed.SeedLobbies(lobbiesRepository, finishedLobbiesRepository, lobbyHub);
                await Seed.SeedLobbyHub(lobbiesRepository, lobbyHub);
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
