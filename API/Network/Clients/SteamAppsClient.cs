using System.Text.Json;
using API.Entities.SteamApps;
using API.Interfaces.IClients;
using API.Network.Clients;

namespace API.Clients
{
    public class SteamAppsClient : BaseClient, ISteamAppsClient
    {
        public SteamAppsClient(HttpClient client) : base(client)
        {
            Client.BaseAddress = new Uri("https://api.steampowered.com/ISteamApps/");
        }

        public async Task<Apps> GetAppsList()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{Client.BaseAddress}GetAppList/v0002");

            using (var response = await Client.SendAsync(request))
            {
                // Ensure we have a Success Status Code
                response.EnsureSuccessStatusCode();

                // Read Response Content (this will usually be JSON content)
                var content = await response.Content.ReadAsStreamAsync();

                // Deserialize the JSON into the C# List<Apps> object and return
                return await JsonSerializer.DeserializeAsync<Apps>(content, JsonSerializerOptions);
            }
        }
    }
}