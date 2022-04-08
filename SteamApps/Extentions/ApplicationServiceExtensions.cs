using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SteamApps.Data;
using SteamApps.Data.Repositories;
using SteamApps.Interfaces.IClients;
using SteamApps.Interfaces.IRepositories;
using SteamApps.Network.Clients;
using SteamApps.Services;

namespace SteamApps.Extentions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddHttpClient<ISteamAppsClient, SteamAppsClient>();
            services.AddHttpClient<ISteamStoreClient, SteamStoreClient>();

            services.AddSingleton<ISteamAppsRepository, SteamAppsRepository>();
            services.AddSingleton<ISteamAppRepository, SteamAppRepository>();

            services.AddHostedService<SteamService>();

            services.AddDbContext<DataContext>(options =>
            {
                var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

                Console.WriteLine(config.GetConnectionString("DefaultConnection"));

                string connStr;

                // Depending on if in development or production, use either MSSQL-provided
                // connection string, or development connection string from env var.
                if (env == "Development")
                {
                    // Use connection string from file.
                    connStr = config.GetConnectionString("DefaultConnection");
                }
                else
                {
                    // Use connection string provided at runtime by Azure.
                    connStr = Environment.GetEnvironmentVariable("DATABASE_URL");
                }

                // Whether the connection string came from the local development configuration file
                // or from the environment variable from Heroku, use it to set up your DbContext.
                options.UseSqlServer(connStr);
            });

            return services;
        }
    }
}