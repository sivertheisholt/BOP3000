using API.Clients;
using API.Data;
using API.Data.Repositories;
using API.Entities.Roles;
using API.Entities.Users;
using API.Interfaces.IClients;
using API.Interfaces.IRepositories;
using API.Interfaces.IRepositories.Steam;
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
                var steamAppsRepository = services.GetRequiredService<ISteamAppsRepository>();
                var steamStoreClient = services.GetRequiredService<ISteamStoreClient>();

                await contex.Database.MigrateAsync();
                await Seed.SeedUsers(userManager, roleManager);
                await Seed.SeedSteamGames(steamAppsRepository, steamAppRepository, steamStoreClient);
            }
            catch (Exception ex)
            {
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
