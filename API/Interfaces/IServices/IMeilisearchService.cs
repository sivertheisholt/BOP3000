using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities.SteamApps;
using Meilisearch;

namespace API.Interfaces.IServices
{
    public interface IMeilisearchService
    {
        Task initializeIndexAsync(AppList appList);

        Task<SearchResult<AppListInfo>> SearchForAppAsync(string title, SearchQuery searchAttributes = null);
    }
}