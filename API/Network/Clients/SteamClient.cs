using System.Text.Json;
using API.Entities.SteamApps;
using API.Interfaces.IClients;

namespace API.Clients
{
    public class SteamClient : ISteamClient
    {
        private readonly HttpClient _client;
        public SteamClient(HttpClient client)
        {
            _client = client;
            _client.BaseAddress = new Uri("https://api.steampowered.com/ISteamApps/");
            _client.DefaultRequestHeaders.Add("Accept", "application/json");
            _client.DefaultRequestHeaders.Clear();
        }

        public async Task<Apps> GetAppsList()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, _client.BaseAddress + "GetAppList/v0002");

            using (var response = await _client.SendAsync(request))
            {
                // Ensure we have a Success Status Code
                response.EnsureSuccessStatusCode();

                // Read Response Content (this will usually be JSON content)
                var content = await response.Content.ReadAsStreamAsync();

                Console.WriteLine(content);

                // Deserialize the JSON into the C# List<Movie> object and return
                return await JsonSerializer.DeserializeAsync<Apps>(content);
            }
        }
    }
}