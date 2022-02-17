using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities.SteamApps;
using API.Interfaces.IServices;
using Meilisearch;

namespace API.Services
{
    public class MeilisearchService : IMeilisearchService
    {
        private readonly MeilisearchClient _meilisearchClient;
        private readonly Meilisearch.Index _index;
        private readonly IConfiguration _config;
        public MeilisearchService(IConfiguration config)
        {
            _config = config;
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            string apiToken;

            if (env == "Development")
            {
                apiToken = config.GetConnectionString("MEILISEARCH_TOKEN_KEY");
            }
            else
            {
                apiToken = Environment.GetEnvironmentVariable("MEILISEARCH_TOKEN_KEY");
            }
            _meilisearchClient = new MeilisearchClient("http://localhost:7700", apiToken);
            _index = _meilisearchClient.Index("apps");
        }

        public async Task initializeIndexAsync(AppList appList)
        {
            var task = await _index.AddDocumentsAsync<AppListInfo>(appList.Apps, "AppListInfoId");
        }

        public async Task<SearchResult<AppListInfo>> SearchForAppAsync(string title, SearchQuery searchAttributes = null)
        {
            return await _index.SearchAsync<AppListInfo>(title, searchAttributes);
        }
    }
}