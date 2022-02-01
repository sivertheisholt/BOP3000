using System.Text.Json;
using API.Entities.SteamApp;
using API.Interfaces.IClients;
using API.Network.Clients;

namespace API.Clients
{
    public class SteamStoreClient : BaseClient, ISteamStoreClient
    {
        public SteamStoreClient(HttpClient client) : base(client)
        {
            Client.BaseAddress = new Uri("https://store.steampowered.com/api/");
        }

        public async Task<GameInfo> GetAppInfo(int appid)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{Client.BaseAddress}appdetails/?appids={appid}");
            using (var response = await Client.SendAsync(request))
            {
                // Ensure we have a Success Status Code
                response.EnsureSuccessStatusCode();

                // Read Response Content (this will usually be JSON content)
                var content = await response.Content.ReadAsStreamAsync();

                // Deserialize the JSON into the C# List<Movie> object and return
                var dictionaryResult = await JsonSerializer.DeserializeAsync<Dictionary<string, GameInfo>>(content, JsonSerializerOptions);
                var game = dictionaryResult.Values.First();
                game.Id = appid;
                return game;
            }
        }
    }
}