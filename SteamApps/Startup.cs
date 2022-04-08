using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationServices(Configuration);
            
            services.AddLogging();
        }
    }
}