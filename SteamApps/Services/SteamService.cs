using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using SteamApps.Entities.SteamApps;
using SteamApps.Helpers;
using SteamApps.Interfaces.IClients;
using SteamApps.Interfaces.IRepositories;
using SteamApps.Interfaces.IServices;

namespace SteamApps.Services
{
    public class SteamService : ISteamService
    {
        private readonly ISteamAppsClient _steamAppsClient;
        private readonly ISteamStoreClient _steamStoreClient;
        private readonly ISteamAppsRepository _steamAppsRepository;
        private readonly ISteamAppRepository _steamAppRepository;
        private readonly IHostApplicationLifetime _hostApplicationLifetime;
        public SteamService(ISteamAppsClient steamAppsClient, ISteamStoreClient steamStoreClient, ISteamAppsRepository steamAppsRepository, ISteamAppRepository steamAppRepository, IHostApplicationLifetime hostApplicationLifetime)
        {
            _hostApplicationLifetime = hostApplicationLifetime;
            _steamAppRepository = steamAppRepository;
            _steamAppsRepository = steamAppsRepository;
            _steamStoreClient = steamStoreClient;
            _steamAppsClient = steamAppsClient;
        }

        public async Task InitDatabase()
        {
            Console.Clear();
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            // connection string, or development connection string from env var.
            if (env == "Production")
            {
                Console.WriteLine("Production environment detected... Starting in 10s", Console.ForegroundColor = ConsoleColor.Red);
                var task = Task.Delay(10000);
                await task;
                await InitProd();
                _hostApplicationLifetime.StopApplication();
            }

            var input = "";
            do
            {
                Console.WriteLine("Is this for testing? y/n", Console.ForegroundColor = ConsoleColor.Red);
                input = Console.ReadLine();
            } while (input != "n" && input != "y");

            if (input == "n")
            {
                do
                {
                    Console.WriteLine("Production server chosen, are you sure? y/n", Console.ForegroundColor = ConsoleColor.Red);
                    input = Console.ReadLine();
                } while (input != "n" && input != "y");

                if (input == "y")
                {
                    Console.WriteLine("Starting initiating of production database...", Console.ForegroundColor = ConsoleColor.Red);
                    await InitProd();
                }
                else
                {
                    _hostApplicationLifetime.StopApplication();
                }
            }
            else
            {
                Console.WriteLine("Starting initiating of testing database...", Console.ForegroundColor = ConsoleColor.Red);
                await InitDev();
            }
        }

        private async Task InitProd()
        {
            var apps = await _steamAppsClient.GetAppsList();
            var appsLoop = new List<AppListInfo>(apps.Apps);

            var counter = 1;
            var max = apps.Apps.Count();
            Console.Clear();

            foreach (var appInfo in appsLoop)
            {
                if (counter == max + 1) break;
                Console.Clear();
                Console.Write($"App ", Console.ForegroundColor = ConsoleColor.Green);
                Console.Write(counter.ToString(), Console.ForegroundColor = ConsoleColor.White);
                Console.Write($" of ", Console.ForegroundColor = ConsoleColor.Green);
                Console.Write($"{max}", Console.ForegroundColor = ConsoleColor.White);
                Console.WriteLine();
                Console.WriteLine($"Getting steam data for game: {appInfo.Name}", Console.ForegroundColor = ConsoleColor.Green);
                Console.WriteLine();

                var app = await _steamStoreClient.GetAppInfo(appInfo.AppId);

                counter++;

                if (!app.Success)
                {
                    Console.WriteLine("Could not get information on app, skipping...", Console.ForegroundColor = ConsoleColor.Red);
                    var taskFailed = Task.Delay(1600);
                    await taskFailed;
                    continue;
                }

                if (app.Data.Type != "game")
                {
                    Console.WriteLine("App is not game, skipping...", Console.ForegroundColor = ConsoleColor.Red);
                }
                else
                {
                    Console.WriteLine("App is game, adding to database...", Console.ForegroundColor = ConsoleColor.DarkGreen);
                }

                var task = Task.Delay(1600);
                await task;
            }

            _steamAppsRepository.AddAppsList(apps);
            await _steamAppsRepository.SaveAllAsync();

            Console.Clear();
            Console.WriteLine("Apps finished sorting! I hope it worked or you waited a very long time for nothing!", Console.ForegroundColor = ConsoleColor.DarkGreen);

            _hostApplicationLifetime.StopApplication();
        }

        private async Task InitDev()
        {
            var apps = await _steamAppsClient.GetAppsList();
            var appsLoop = new List<AppListInfo>(apps.Apps);

            var counter = 1;
            var max = 50;
            Console.Clear();
            foreach (var appInfo in appsLoop)
            {
                if (counter == max + 1) break;
                Console.Clear();
                Console.Write($"App ", Console.ForegroundColor = ConsoleColor.Green);
                Console.Write(counter.ToString(), Console.ForegroundColor = ConsoleColor.White);
                Console.Write($" of ", Console.ForegroundColor = ConsoleColor.Green);
                Console.Write($"{max}", Console.ForegroundColor = ConsoleColor.White);
                Console.WriteLine();
                Console.WriteLine($"Getting steam data for game: {appInfo.Name}", Console.ForegroundColor = ConsoleColor.Green);
                Console.WriteLine();

                var app = await _steamStoreClient.GetAppInfo(appInfo.AppId);

                counter++;

                if (!app.Success)
                {
                    Console.WriteLine("Could not get information on app, skipping...", Console.ForegroundColor = ConsoleColor.Red);
                    apps.Apps.Remove(appInfo);
                    var taskFailed = Task.Delay(1000);
                    await taskFailed;
                    continue;
                }

                if (app.Data.Type != "game")
                {
                    Console.WriteLine("App is not game, skipping...", Console.ForegroundColor = ConsoleColor.Red);
                    apps.Apps.Remove(appInfo);
                }
                else
                {
                    Console.WriteLine("App is game, adding to database...", Console.ForegroundColor = ConsoleColor.DarkGreen);
                    _steamAppRepository.AddApp(app);
                }

                var task = Task.Delay(1000);
                await task;

                if (counter % 10 == 0)
                {
                    Console.WriteLine("Stack reached, saving changes to database...", Console.ForegroundColor = ConsoleColor.DarkGreen);
                    await _steamAppRepository.SaveAllAsync();
                }
            }
            await _steamAppRepository.SaveAllAsync();
            _steamAppsRepository.AddAppsList(apps);
            await _steamAppsRepository.SaveAllAsync();

            Console.Clear();
            Console.WriteLine("Apps finished sorting! I hope it worked or you waited a very long time for nothing!", Console.ForegroundColor = ConsoleColor.DarkGreen);

            _hostApplicationLifetime.StopApplication();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await InitDatabase();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}