using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities.SteamApp.Information;
using API.Interfaces.IClients;
using API.Interfaces.IRepositories;
using API.Interfaces.IServices;

namespace API.Data
{
    public class InitDatabase
    {
        public static async Task InitAppList(ISteamAppsRepository steamAppsRepository, ISteamAppsClient steamAppsClient, IMeilisearchService meilisearchService)
        {
            if (await steamAppsRepository.GetAppsInfoAsync(1) != null) return;

            var apps = await steamAppsClient.GetAppsList();
            steamAppsRepository.AddAppsList(apps);

            
        }
    }
}