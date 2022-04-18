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
        private readonly IConfiguration _config;
        private Dictionary<string, Meilisearch.Index> Indexes = new Dictionary<string, Meilisearch.Index>();
        public MeilisearchService(IConfiguration config)
        {
            _config = config;
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            string apiToken;
            string url;

            if (env == "Development")
            {
                apiToken = config.GetConnectionString("MEILISEARCH_TOKEN_KEY");
                url = "http://localhost:7700";
            }
            else
            {
                apiToken = Environment.GetEnvironmentVariable("MEILISEARCH_TOKEN_KEY");
                url = Environment.GetEnvironmentVariable("MEILISEARCH_URL");
            }
            _meilisearchClient = new MeilisearchClient(url, apiToken);
            InitializeIndexes();
        }

        private async void InitializeIndexes()
        {
            var indexes = await _meilisearchClient.GetAllIndexesAsync();
            foreach (var item in indexes)
            {
                Indexes.Add(item.Uid, item);
            }
        }

        public Meilisearch.Index GetIndex(string indexName)
        {
            var index = Indexes.FirstOrDefault(index => index.Key == indexName).Value;
            return index;
        }

        public Task CreateIndexAsync(string indexName)
        {
            if (GetIndex(indexName) != null) return Task.CompletedTask;
            var result = _meilisearchClient.CreateIndexAsync(indexName);

            var continuation = result.ContinueWith(x =>
            {
                var index = _meilisearchClient.Index(indexName);
                Indexes.Add(indexName, index);
            });
            continuation.Wait();
            return Task.CompletedTask;
        }

        public async Task AddDocumentsAsync<T>(T[] entities, Meilisearch.Index index)
        {
            await index.AddDocumentsAsync<T>(entities);
        }

        public async Task<SearchResult<T>> SearchAsync<T>(Meilisearch.Index index, string title, SearchQuery searchAttributes = null)
        {
            return await index.SearchAsync<T>(title, searchAttributes);
        }

    }
}