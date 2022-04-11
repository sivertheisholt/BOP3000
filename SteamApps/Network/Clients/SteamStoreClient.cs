using SteamApps.Entities.SteamApp;
using SteamApps.Interfaces.IClients;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SteamApps.Network.Clients
{
    public class SteamStoreClient : BaseClient, ISteamStoreClient
    {
        public SteamStoreClient(HttpClient client) : base(client)
        {
            Client.BaseAddress = new Uri("https://store.steampowered.com/api/");
        }

        public async Task<AppInfo> GetAppInfo(int appid)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{Client.BaseAddress}appdetails/?appids={appid}");
            using (var response = await Client.SendAsync(request))
            {
                // Ensure we have a Success Status Code
                try
                {
                    response.EnsureSuccessStatusCode();

                    // Read Response Content (this will usually be JSON content)
                    var content = await response.Content.ReadAsStringAsync();

                    JObject contentObject = JObject.Parse(content);

                    // Check if data is present
                    if (contentObject[appid.ToString()]["data"] == null)
                    {
                        return new AppInfo
                        {
                            Success = false
                        };
                    };

                    // For some reason, some games treat this as an array?
                    // For example, App ID 380 has an empty array here.
                    // I don't know how to simultaneously serialize this to an object and an array, so just ignore the weird arrays.
                    var linuxRequirementsToken = contentObject[appid.ToString()]["data"]["linux_requirements"];
                    if (linuxRequirementsToken != null && linuxRequirementsToken.Type == JTokenType.Array)
                    {
                        contentObject[appid.ToString()]["data"]["linux_requirements"] = null;
                    }

                    // Same here - For pc
                    var pcRequirementsToken = contentObject[appid.ToString()]["data"]["pc_requirements"];
                    if (pcRequirementsToken != null && pcRequirementsToken.Type == JTokenType.Array)
                    {
                        contentObject[appid.ToString()]["data"]["pc_requirements"] = null;
                    }

                    // Same here - Same here for mac
                    var macRequirementsToken = contentObject[appid.ToString()]["data"]["mac_requirements"];
                    if (macRequirementsToken != null && macRequirementsToken.Type == JTokenType.Array)
                    {
                        contentObject[appid.ToString()]["data"]["mac_requirements"] = null;
                    }

                    var resultString = JsonConvert.SerializeObject(contentObject);

                    // Deserialize the JSON string
                    var dictionaryResult = JsonConvert.DeserializeObject<Dictionary<string, AppInfo>>(resultString);

                    var game = dictionaryResult.Values.First();

                    return game;
                }
                catch (System.Exception)
                {
                    Console.WriteLine("The fuck?");
                    return new AppInfo
                    {
                        Success = false
                    };
                }
            }
        }
    }
}