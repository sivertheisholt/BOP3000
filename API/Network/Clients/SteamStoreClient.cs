using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text.Json;
using System.Threading.Tasks;
using API.Entities.SteamGame;
using API.Interfaces.IClients;

namespace API.Clients
{
    public class SteamStoreClient : ISteamStoreClient
    {
        private readonly HttpClient _client;
        public SteamStoreClient(HttpClient client)
        {
            _client = client;
            _client.BaseAddress = new Uri("https://store.steampowered.com/api/");
            _client.DefaultRequestHeaders.Add("Accept", "application/json");
            _client.DefaultRequestHeaders.Clear();
        }

        public async Task<GameInfo> GetAppInfo(string appid)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{_client.BaseAddress}appdetails/?appids={appid}");
            using (var response = await _client.SendAsync(request))
            {
                // Ensure we have a Success Status Code
                response.EnsureSuccessStatusCode();

                // Read Response Content (this will usually be JSON content)
                var content = await response.Content.ReadAsStreamAsync();

                // Deserialize the JSON into the C# List<Movie> object and return
                var dictionaryResult = await JsonSerializer.DeserializeAsync<Dictionary<string, GameInfo>>(content);
                var game = dictionaryResult.Values.First();
                return game;
            }
        }
    }
}