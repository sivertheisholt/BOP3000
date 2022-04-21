using API.Clients;
using API.Data;
using API.Data.Repositories;
using API.Helpers;
using API.Interfaces;
using API.Interfaces.IClients;
using API.Interfaces.IRepositories;
using API.Interfaces.IServices;
using API.Network.Clients;
using API.Services;
using API.SignalR;
using Microsoft.EntityFrameworkCore;

namespace API.Extentions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<CloudinarySettings>(config.GetSection("CLOUDINARY_SETTINGS"));

            services.AddScoped<LobbyHub>();
            services.AddScoped<LobbyChatHub>();
            services.AddSingleton<LobbyTracker>();
            services.AddSingleton<LobbyChatTracker>();

            services.AddHttpClient<ISteamAppsClient, SteamAppsClient>();
            services.AddHttpClient<ISteamStoreClient, SteamStoreClient>();
            services.AddHttpClient<IDiscordApiClient, DiscordApiClient>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();


            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IPhotoService, PhotoService>();
            services.AddSingleton<IEmailService, EmailService>();
            services.AddSingleton<IMeilisearchService, MeilisearchService>();
            services.AddScoped<IFileService, FileService>();


            services.AddSingleton<IDiscordBotService, DiscordBotService>();

            services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);

            services.AddDbContext<DataContext>(options =>
            {
                var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

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