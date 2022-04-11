using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SteamApps;
using SteamApps.Data;
using SteamApps.Data.Repositories;
using SteamApps.Extentions;
using SteamApps.Interfaces.IClients;
using SteamApps.Interfaces.IRepositories;
using SteamApps.Interfaces.IServices;
using SteamApps.Network.Clients;
using SteamApps.Services;

namespace SteamApps
{
    public class Program
    {

        static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .UseStartup<Startup>(); // our new method!
    }
}