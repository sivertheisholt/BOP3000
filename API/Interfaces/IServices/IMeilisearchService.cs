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
        Task<SearchResult<T>> SearchAsync<T>(Meilisearch.Index index, string title, SearchQuery searchAttributes = null);

        Meilisearch.Index GetIndex(string indexName);

        Task CreateIndexAsync(string indexName);

        Task AddDocumentsAsync<T>(T[] entities, Meilisearch.Index index);

        Task DeleteAllDocumentsAsync<T>(Meilisearch.Index index);
    }
}