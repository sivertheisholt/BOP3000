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
        public MeilisearchService()
        {
            _meilisearchClient = new MeilisearchClient("http://localhost:7700");
            _index = _meilisearchClient.Index("apps");
        }

        public async Task initializeIndexAsync(AppList appList)
        {
            var task = await _index.AddDocumentsAsync<AppListInfo>(appList.Apps, "AppListInfoId");
        }
    }
}