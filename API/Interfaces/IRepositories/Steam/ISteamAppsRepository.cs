using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities.SteamApps;

namespace API.Interfaces.IRepositories.Steam
{
    public interface ISteamAppsRepository
    {
        Task<Apps> GetAppsAsync();
    }
}