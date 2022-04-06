using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs.Applications;
using API.Interfaces.IClients;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace API.Network.Clients
{
    public class DiscordApiClient : BaseClient, IDiscordApiClient
    {
        public DiscordApiClient(HttpClient client) : base(client)
        {
            Client.BaseAddress = new Uri("https://discord.com/api");
        }

        public async Task<DiscordUserDto> GetUserObjectFromToken(string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{Client.BaseAddress}/users/@me");
            request.Headers.Add("Authorization", "Bearer " + token);
            using (var response = await Client.SendAsync(request))
            {
                // Ensure we have a Success Status Code
                try
                {
                    response.EnsureSuccessStatusCode();
                }
                catch (System.Exception)
                {
                    Console.WriteLine("Something wrong happen");
                }

                // Read Response Content (this will usually be JSON content)
                var content = await response.Content.ReadAsStringAsync();

                JObject contentObject = JObject.Parse(content);

                var resultString = JsonConvert.SerializeObject(contentObject);

                // Deserialize the JSON string
                var userResult = JsonConvert.DeserializeObject<DiscordUserDto>(resultString);

                Console.WriteLine(userResult);

                return userResult;
            }
        }
    }
}