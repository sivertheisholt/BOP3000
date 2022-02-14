using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities.SteamApps;

namespace API.Interfaces.IServices
{
    public interface IMeilisearchService
    {
        Task initializeIndexAsync(AppList appList);
    }
}