using System.Text.Json;
using API.Entities.SteamApps;
using API.Interfaces.IClients;
using API.Network.Clients;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace API.Clients
{
    public class SteamAppsClient : BaseClient, ISteamAppsClient
    {
        public SteamAppsClient(HttpClient client) : base(client)
        {
            Client.BaseAddress = new Uri("https://api.steampowered.com/ISteamApps/");
        }

        public async Task<AppList> GetAppsList()
        {

            var request = new HttpRequestMessage(HttpMethod.Get, $"{Client.BaseAddress}GetAppList/v0002");
            using (var response = await Client.SendAsync(request))
            {
                // Ensure we have a Success Status Code
                try
                {
                    response.EnsureSuccessStatusCode();
                }
                catch (System.Exception)
                {

                    return new AppList
                    {
                        Success = false
                    };
                }

                // Read Response Content (this will usually be JSON content)
                var content = await response.Content.ReadAsStringAsync();

                JObject contentObject = JObject.Parse(content);

                //Check if data is present
                if (contentObject["applist"] == null)
                {
                    return new AppList
                    {
                        Success = false
                    };
                };

                var resultString = JsonConvert.SerializeObject(contentObject["applist"]);

                // Deserialize the JSON string
                var appsResult = JsonConvert.DeserializeObject<AppList>(resultString);
                appsResult.Success = true;

                Console.WriteLine(appsResult);

                return appsResult;
            }
        }
    }
}